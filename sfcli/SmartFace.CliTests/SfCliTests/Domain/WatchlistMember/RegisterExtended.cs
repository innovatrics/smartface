using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using Tests;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistMember
{
    public class RegisterExtendedTestContext
    {
        public ILogger<WatchlistMemberRegistrationManager> Logger { get; }

        public WatchlistMemberRegistrationManager Manager { get; }

        public IWatchlistMembersRepository Repository { get; }

        public List<RegisterWatchlistMemberData> RegisteredData { get; } = new List<RegisterWatchlistMemberData>();

        public string Dir { get; }

        public RegisterExtendedTestContext()
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

        public string GetFileContent(string filePath)
        {
            string content =@"
[
{
  ""ExternalId"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""" + filePath + @"""]
} 
]
";
            return content;
        }

        public string CreatePhotoFile(string photoFileName)
        {
            string photoFileFullPath = Path.Combine(Dir, photoFileName);
            File.WriteAllBytes(photoFileFullPath, Encoding.UTF8.GetBytes(photoFileName));
            return photoFileFullPath;
        }

        public void CreateDataFile(string photoPath)
        {
            string content = GetFileContent(photoPath);
            string dataFileFullPath = Path.Combine(Dir, "extendedData.json");
            File.WriteAllText(dataFileFullPath, content, Encoding.UTF8);
        }
    }

    public class WatchlistMemberRegistration_RelativePhotoPath : Scenario<RegisterExtendedTestContext>
    {
        private given _files = testContext =>
        {
            string photoFileName = "relativePhoto.jpeg";
            testContext.CreatePhotoFile(photoFileName);
            testContext.CreateDataFile(photoFileName);
        };

        private whenAsync _register = testContext => testContext.Manager.RegisterWatchlistMembersExtendedFromDirAsync(testContext.Dir, new[] {"Wl"}, 1);

        private then _photoWithRelativePathFound = testContext =>
        {
            Assert.AreEqual(1, testContext.RegisteredData.Count);
            var registered = testContext.RegisteredData.Single();
            Assert.AreEqual("120", registered.ExternalId);
            Assert.AreEqual(1, registered.ImageData.Count);
            var imageData = registered.ImageData.Single();
            Assert.AreEqual(Constants.JPEG_MIME_TYPE, imageData.MIME);
            Assert.AreEqual("relativePhoto.jpeg", Encoding.UTF8.GetString(imageData.Data));
        };
    }

    public class WatchlistMemberRegistration_AbsolutePhotoPath : Scenario<RegisterExtendedTestContext>
    {
        private given _files = testContext =>
        {
            string photoFileName = "absolutePhoto.jpeg";
            var absolutePathToPhotoFile = testContext.CreatePhotoFile(photoFileName);
            testContext.CreateDataFile(absolutePathToPhotoFile.Replace(@"\", @"\\"));
        };

        private whenAsync _register = testContext => testContext.Manager.RegisterWatchlistMembersExtendedFromDirAsync(testContext.Dir, new[] {"Wl"}, 1);

        private then _photoWithAbsolutePathFound = testContext =>
        {
            Assert.AreEqual(1, testContext.RegisteredData.Count);
            var registered = testContext.RegisteredData.Single();
            Assert.AreEqual("120", registered.ExternalId);
            Assert.AreEqual(1, registered.ImageData.Count);
            var imageData = registered.ImageData.Single();
            Assert.AreEqual(Constants.JPEG_MIME_TYPE, imageData.MIME);
            Assert.AreEqual("absolutePhoto.jpeg", Encoding.UTF8.GetString(imageData.Data));
        };
    }
}
