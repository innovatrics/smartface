using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.Utils;

namespace SmartFace.Cli.Core.Domain.ImgExport.Impl
{
    public class ExportFileResolver : IExportFileResolver
    {
        private ILogger<ExportFileResolver> Log { get; }
        
        private readonly string[] _dynamicProperties;

        private string ImageUrlProperty { get; }
        
        private string ResultFileName { get; }
        

        public ExportFileResolver(string resultFileName, string imageUrlProperty)
        {
            ResultFileName = resultFileName;
            ImageUrlProperty = imageUrlProperty;
            _dynamicProperties = ParseDynamicProperties(resultFileName);
        }

        public IEnumerable<ExportFile> Resolve(IEnumerable entities)
        {
            List<ExportFile> files = new List<ExportFile>();
            foreach (var entity in entities)
            {
                var filesForEntities = ResolveExportFiles(entity, _dynamicProperties);
                files.AddRange(filesForEntities);
            }

            return files;
        }

        private static string[] ParseDynamicProperties(string format)
        {
            var regex = new Regex(@"(?<=\{)[^}]*(?=\})");
            var matchCollection = regex.Matches(format);
            var formatProperties = matchCollection.Select(mc => mc.Groups.First<Group>().Value).ToArray();
            return formatProperties;
        }

        private static Dictionary<string, object[]> ResolveDynamicProperties(string[] formatProperties, object entity,
            dynamic[] imgUrls)
        {
            var propValuesByName = new Dictionary<string, object[]>();
            foreach (var formatProperty in formatProperties)
            {
                var formatPropertyValues = entity.GetValuesOnPropertyPath(formatProperty).ToDynamicArray();
                if (formatPropertyValues.Length == 1)
                {
                    object[] values = new object[imgUrls.Length];
                    for (int i = 0; i < imgUrls.Length; i++)
                    {
                        values[i] = formatPropertyValues.Single();
                    }

                    formatPropertyValues = values;
                }

                if (imgUrls.Length != formatPropertyValues.Length)
                {
                    throw new ProcessingException(
                        "Unable to build file name. Count of images does not equal to formatting properties objects.");
                }

                propValuesByName[formatProperty] = formatPropertyValues;
            }

            return propValuesByName;
        }

        private List<ExportFile> ResolveExportFiles(object entity, string[] formatProperties)
        {
            var imgUrls = entity.GetValuesOnPropertyPath(ImageUrlProperty).ToDynamicArray();

            var propValuesByName = ResolveDynamicProperties(formatProperties, entity, imgUrls);

            List<ExportFile> resultDefinitions = new List<ExportFile>();
            for (var i = 0; i < imgUrls.Length; i++)
            {
                //assemble file name
                var fileName = ResultFileName;
                foreach (var property in formatProperties)
                {
                    var values = propValuesByName[property];
                    fileName = fileName.Replace($"{{{property}}}", values[i].ToString());
                }

                var imgUrl = (string) imgUrls[i];
                resultDefinitions.Add(new ExportFile(fileName, imgUrl));
            }

            return resultDefinitions;
        }
    }
}