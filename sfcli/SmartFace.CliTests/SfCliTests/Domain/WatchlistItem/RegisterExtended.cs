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
using SmartFace.Cli.Core.Domain.WatchlistItem.Impl;
using Tests;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistItem
{
    public class RegisterExtendedTestContext
    {
        public ILogger<WatchlistItemRegistrationManager> Logger { get; }

        public WatchlistItemRegistrationManager Manager { get; }

        public IWlItemsRepository Repository { get; }

        public List<RegisterWlItemData> RegisteredData { get; } = new List<RegisterWlItemData>();

        public string Dir { get; }

        public RegisterExtendedTestContext()
        {
            var logger = Substitute.For<ILogger<WatchlistItemRegistrationManager>>();
            Logger = logger;
            Repository = Substitute.For<IWlItemsRepository>();
            var loaderLogger = Substitute.For<ILogger<RegisterWlItemExtendedJsonLoader>>();
            var loader = new RegisterWlItemExtendedJsonLoader(loaderLogger);
            Manager = new WatchlistItemRegistrationManager(logger, Repository, loader);
            Dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Dir);

            Repository.WhenForAnyArgs(r => r.Register(null)).Do(info => RegisteredData.Add(info.Arg<RegisterWlItemData>()));
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

    public class WatchlistItemRegistration_RelativePhotoPath : Scenario<RegisterExtendedTestContext>
    {
        private given _files = testContext =>
        {
            string photoFileName = "relativePhoto.jpeg";
            testContext.CreatePhotoFile(photoFileName);
            testContext.CreateDataFile(photoFileName);
        };

        private when _register = testContext => testContext.Manager.RegisterWlItemsExtendedFromDir(testContext.Dir, new[] {"Wl"});

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

    public class WatchlistItemRegistration_AbsolutePhotoPath : Scenario<RegisterExtendedTestContext>
    {
        private given _files = testContext =>
        {
            string photoFileName = "absolutePhoto.jpeg";
            var absolutePathToPhotoFile = testContext.CreatePhotoFile(photoFileName);
            testContext.CreateDataFile(absolutePathToPhotoFile.Replace(@"\", @"\\"));
        };

        private when _register = testContext => testContext.Manager.RegisterWlItemsExtendedFromDir(testContext.Dir, new[] {"Wl"});

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
