using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;
using Tests;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistMember
{
    public class JsonLoaderContext
    {
        public ILogger<RegisterWatchlistMemberExtendedJsonLoader> Logger { get; }

        public RegisterWatchlistMemberExtendedJsonLoader Loader { get; }

        public string Dir { get; }

        public RegisterWatchlistMemberExtended[] SerializedData { get; set; }

        public JsonLoaderContext()
        {
            var logger = Substitute.For<ILogger<RegisterWatchlistMemberExtendedJsonLoader>>();
            Logger = logger;
            Loader = new RegisterWatchlistMemberExtendedJsonLoader(logger);
            Dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Dir);
        }

    }

    public class WatchlistMemberRegistration_LoadJson : Scenario<JsonLoaderContext>
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

        private when _load = testContext => testContext.SerializedData = testContext.Loader.GetRegisterWatchlistMemberExtendedData(testContext.Dir);

        private then _thereAreTwoMembers = testContext => Assert.AreEqual(2, testContext.SerializedData.Length);
        private then _memberWithExternalId120LoadedCorrectly = testContext =>
        {
            var member = testContext.SerializedData.Single(itm => itm.ExternalId == "120");
            Assert.AreEqual("Display name", member.DisplayName);
            Assert.AreEqual("Full name", member.FullName);
            Assert.AreEqual("Example note", member.Note);
            Assert.AreEqual("file1.jpeg", member.PhotoFiles.First());
            Assert.AreEqual("file2.jpeg", member.PhotoFiles.Last());
        };
    }
}
