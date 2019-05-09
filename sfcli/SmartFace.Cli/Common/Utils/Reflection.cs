using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace SmartFace.Cli.Common.Utils
{
    public static class Reflection
    {
        public static IEnumerable GetValuesOnPropertyPath(this object entity, string nestedProperty)
        {
            char separator = '.';
            var properties = nestedProperty.Split(separator);
            var propertyName = properties.First();
            var result = entity.GetType().GetProperty(propertyName).GetValue(entity);
            if (properties.Length == 1)
            {
                return new[] {result};
            }

            if (!(result is string) && result is IEnumerable results)
            {
                var multiResult = new List<object>();
                foreach (var single in results)
                {
                    IEnumerable result2Level =
                        GetValuesOnPropertyPath(single, string.Join(separator, properties.Skip(1)));
                    multiResult.AddRange(result2Level.ToDynamicList());
                }

                return multiResult;
            }

            return GetValuesOnPropertyPath(result, string.Join(separator, properties.Skip(1)));
        }

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }

                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }

                if (!targetProperty.CanWrite)
                {
                    continue;
                }

                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }

                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }

                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}