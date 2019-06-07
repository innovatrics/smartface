using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SmartFace.Cli.Core.Domain.WatchlistItem.Impl;
using SmartFace.Cli.Core.Domain.WatchlistItem.Model;
using Tests;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistItem
{
    public class JsonLoaderContext
    {
        public ILogger<RegisterWlItemExtendedJsonLoader> Logger { get; }

        public RegisterWlItemExtendedJsonLoader Loader { get; }

        public string Dir { get; }

        public RegisterWlItemExtended[] SerializedData { get; set; }

        public JsonLoaderContext()
        {
            var logger = Substitute.For<ILogger<RegisterWlItemExtendedJsonLoader>>();
            Logger = logger;
            Loader = new RegisterWlItemExtendedJsonLoader(logger);
            Dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Dir);
        }

    }

    public class WatchlistItemRegistration_LoadJson : Scenario<JsonLoaderContext>
    {
        private given _jsonFile = testContext =>
        {
            string content = @"
[
{
  ""ExternalId"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""file1.jpeg"", ""file2.jpeg""]
}, 
{
  ""ExternalId"": ""121"",
    ""DisplayName"": ""Display name2"",
    ""FullName"": ""Full name2"",
    ""Note"": ""Example note2"",
    ""PhotoFiles"": [""file1a.jpeg"", ""file2a.jpeg""]
} 
]
";
            string fileName = Path.Combine(testContext.Dir, "file_with_inconsistent_case_in_extension.JsoN");
            File.WriteAllText(fileName, content, Encoding.UTF8);
        };

        private when _load = testContext => testContext.SerializedData = testContext.Loader.GetRegisterWlItemExtendedData(testContext.Dir);

        private then _thereAreTwoItems = testContext => Assert.AreEqual(2, testContext.SerializedData.Length);
        private then _itemWithExternalId120LoadedCorrectly = testContext =>
        {
            var item = testContext.SerializedData.Single(itm => itm.ExternalId == "120");
            Assert.AreEqual("Display name", item.DisplayName);
            Assert.AreEqual("Full name", item.FullName);
            Assert.AreEqual("Example note", item.Note);
            Assert.AreEqual("file1.jpeg", item.PhotoFiles.First());
            Assert.AreEqual("file2.jpeg", item.PhotoFiles.Last());
        };
    }
}
