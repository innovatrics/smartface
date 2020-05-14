using System.Linq;
using System.Linq.Dynamic.Core;
using SmartFace.Cli.Common.Utils;
using Xunit;

namespace SmartFace.CliTests.SfCliTests.Utils
{
    public class ReflectionTest
    {
        [Fact]
        public void GetValuesOnPropertyPath_FromSimpleObject()
        {
            var source = new {Name = "abc"};

            var destination = source.GetValuesOnPropertyPath("Name");

            Assert.Equal("abc", destination.ToDynamicArray().Single() as string);
        }

        [Fact]
        public void GetValuesOnPropertyPath_NestedObject()
        {
            var source = new {Level1 = new {Name = "abc"}};

            var destination = source.GetValuesOnPropertyPath("Level1.Name");

            Assert.Equal("abc", destination.ToDynamicArray().Single() as string);
        }

        [Fact]
        public void GetValuesOnPropertyPath_FromNestedSimpleObjectArray()
        {
            var source = new {Arr = new[] {new {Name = "abc"}, new {Name = "xyz"}}};

            var destination = source.GetValuesOnPropertyPath("Arr.Name");

            Assert.Equal("abc", destination.ToDynamicArray().First() as string);
            Assert.Equal("xyz", destination.ToDynamicArray().Last() as string);
        }

        [Fact]
        public void GetValuesOnPropertyPath_FromSecondLevelNestedSimpleObjectArray()
        {
            var source = new {Arr1 = new {Arr2 = new[] {new {Name = "abc"}, new {Name = "xyz"}}}};
            var destination = source.GetValuesOnPropertyPath("Arr1.Arr2.Name");

            Assert.Equal("abc", destination.ToDynamicArray().First() as string);
            Assert.Equal("xyz", destination.ToDynamicArray().Last() as string);
        }
    }
}