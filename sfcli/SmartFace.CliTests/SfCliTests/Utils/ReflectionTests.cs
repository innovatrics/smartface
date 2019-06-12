using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using NUnit.Framework;
using SmartFace.Cli.Common.Utils;
using Tests;

namespace SmartFace.CliTests.SfCliTests.Utils
{
    public class ReflectionContext
    {
        public object Source { get; set; }
        
        public IEnumerable Destination { get; set; }
    }
    
    public class GetValuesOnPropertyPath_FromSimpleObject : Scenario<ReflectionContext>
    {
        private given _simple_object = _ => _.Source = new {Name = "abc"};

        private when _read_name = _ => _.Destination = _.Source.GetValuesOnPropertyPath("Name");

        private then _destination_contain_name_value = _ => Assert.IsTrue(_.Destination.ToDynamicArray().Single() as string == "abc");
    }
    
    public class GetValuesOnPropertyPath_NestedObject : Scenario<ReflectionContext>
    {
        private given _simple_object = _ => _.Source = new { Level1 = new {Name = "abc"}};

        private when _read_name = _ => _.Destination = _.Source.GetValuesOnPropertyPath("Level1.Name");

        private then _destination_contain_name_value = _ => Assert.IsTrue(_.Destination.ToDynamicArray().Single() as string == "abc");
    }
    
    public class GetValuesOnPropertyPath_FromNestedSimpleObjectArray : Scenario<ReflectionContext>
    {
        private given _simple_object = _ => _.Source = new { Arr = new []{ new {Name = "abc"}, new {Name = "xyz"} }};

        private when _read_name = _ => _.Destination = _.Source.GetValuesOnPropertyPath("Arr.Name");

        private then _destination_contain_first_name_value = _ => Assert.IsTrue(_.Destination.ToDynamicArray().First() as string == "abc", "first does not equal");
        private then _destination_contain_last_name_value = _ => Assert.IsTrue(_.Destination.ToDynamicArray().Last() as string == "xyz", "last does not equal");
    }
    
    public class GetValuesOnPropertyPath_FromSecondLevelNestedSimpleObjectArray : Scenario<ReflectionContext>
    {
        private given _simple_object = _ => _.Source = new { Arr1 = new { Arr2 = new []{ new {Name = "abc"}, new {Name = "xyz"} }}};

        private when _read_name = _ => _.Destination = _.Source.GetValuesOnPropertyPath("Arr1.Arr2.Name");

        private then _destination_contain_first_name_value = _ => Assert.IsTrue(_.Destination.ToDynamicArray().First() as string == "abc", "first does not equal");
        private then _destination_contain_last_name_value = _ => Assert.IsTrue(_.Destination.ToDynamicArray().Last() as string == "xyz", "last does not equal");
    }
}