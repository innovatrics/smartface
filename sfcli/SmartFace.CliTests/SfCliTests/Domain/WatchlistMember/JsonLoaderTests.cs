using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using Xunit;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistMember
{
    public class JsonLoaderTests
    {
        [Fact]
        public void WatchlistMemberRegistration_LoadJson()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            string content = @"
[
{
  ""Id"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""file1.jpeg"", ""file2.jpeg""]
}, 
{
  ""Id"": ""121"",
    ""DisplayName"": ""Display name2"",
    ""FullName"": ""Full name2"",
    ""Note"": ""Example note2"",
    ""PhotoFiles"": [""file1a.jpeg"", ""file2a.jpeg""]
} 
]
";
            string fileName = Path.Combine(dir, "file_with_inconsistent_case_in_extension.JsoN");
            File.WriteAllText(fileName, content, Encoding.UTF8);

            var logger = Substitute.For<ILogger<WatchlistMemberRegistrationDataJsonLoader>>();
            var loader = new WatchlistMemberRegistrationDataJsonLoader(logger);

            var serializeData = loader.GetWatchlistMemberRegistrationData(dir);

            Assert.Equal(2, serializeData.Length);

            var member = serializeData.Single(itm => itm.Id == "120");
            Assert.Equal("Display name", member.DisplayName);
            Assert.Equal("Full name", member.FullName);
            Assert.Equal("Example note", member.Note);
            Assert.Equal("file1.jpeg", member.PhotoFiles.First());
            Assert.Equal("file2.jpeg", member.PhotoFiles.Last());
        }
    }
}
