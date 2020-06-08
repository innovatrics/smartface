using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using Xunit;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistMember
{
    public class RegisterExtendedTests
    {
        public ILogger<WatchlistMemberRegistrationManager> Logger { get; }
        public WatchlistMemberRegistrationManager Manager { get; }
        public IWatchlistMembersRepository Repository { get; }
        public List<RegisterWatchlistMemberData> RegisteredData { get; } = new List<RegisterWatchlistMemberData>();
        public string Dir { get; }

        public RegisterExtendedTests()
        {
            var logger = Substitute.For<ILogger<WatchlistMemberRegistrationManager>>();
            Logger = logger;
            Repository = Substitute.For<IWatchlistMembersRepository>();
            var loaderLogger = Substitute.For<ILogger<RegisterWatchlistMemberExtendedJsonLoader>>();
            var loader = new RegisterWatchlistMemberExtendedJsonLoader(loaderLogger);
            Manager = new WatchlistMemberRegistrationManager(logger, Repository, loader);
            Dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Dir);

            Repository.WhenForAnyArgs(r => r.RegisterAsync(null)).Do(info => RegisteredData.Add(info.Arg<RegisterWatchlistMemberData>()));
        }

        [Fact]
        public async Task WatchlistMemberRegistration_RelativePhotoPath()
        {
            string photoFileName = "relativePhoto.jpeg";
            CreatePhotoFile(photoFileName);
            CreateDataFile(photoFileName);

            await Manager.RegisterWatchlistMembersExtendedFromDirAsync(Dir, new[] { "Wl" }, 1);

            Assert.Single(RegisteredData);
            var registered = RegisteredData.Single();
            Assert.Equal("120", registered.Id);
            Assert.Single(registered.ImageData);
            var imageData = registered.ImageData.Single();
            Assert.Equal("relativePhoto.jpeg", Encoding.UTF8.GetString(imageData.Data));
        }

        [Fact]
        public async Task WatchlistMemberRegistration_AbsolutePhotoPath()
        {

            string photoFileName = "absolutePhoto.jpeg";
            var absolutePathToPhotoFile = CreatePhotoFile(photoFileName);
            CreateDataFile(absolutePathToPhotoFile.Replace(@"\", @"\\"));

            await Manager.RegisterWatchlistMembersExtendedFromDirAsync(Dir, new[] { "Wl" }, 1);

            Assert.Single(RegisteredData);
            var registered = RegisteredData.Single();
            Assert.Equal("120", registered.Id);
            Assert.Single(registered.ImageData);
            var imageData = registered.ImageData.Single();
            Assert.Equal("absolutePhoto.jpeg", Encoding.UTF8.GetString(imageData.Data));
        }

        private string CreatePhotoFile(string photoFileName)
        {
            string photoFileFullPath = Path.Combine(Dir, photoFileName);
            File.WriteAllBytes(photoFileFullPath, Encoding.UTF8.GetBytes(photoFileName));
            return photoFileFullPath;
        }

        private void CreateDataFile(string photoPath)
        {
            string content = GetFileContent(photoPath);
            string dataFileFullPath = Path.Combine(Dir, "extendedData.json");
            File.WriteAllText(dataFileFullPath, content, Encoding.UTF8);
        }

        public string GetFileContent(string filePath)
        {
            string content = @"
[
{
  ""Id"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""" + filePath + @"""]
} 
]
";
            return content;
        }
    }
}
