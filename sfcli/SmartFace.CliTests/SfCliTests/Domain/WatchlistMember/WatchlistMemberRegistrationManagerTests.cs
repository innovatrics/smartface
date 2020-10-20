using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using SmartFace.Cli.Commands.SubWatchlistMember;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using Xunit;

namespace SmartFace.CliTests.SfCliTests.Domain.WatchlistMember
{
    public class WatchlistMemberRegistrationManagerTests
    {
        public ILogger<WatchlistMemberRegistrationManager> Logger { get; }
        public WatchlistMemberRegistrationManager Manager { get; }
        public IWatchlistMembersRepository Repository { get; }
        public string RegistrationDir { get; }

        public WatchlistMemberRegistrationManagerTests()
        {
            var logger = Substitute.For<ILogger<WatchlistMemberRegistrationManager>>();
            Logger = logger;
            Repository = Substitute.For<IWatchlistMembersRepository>();
            var loaderLogger = Substitute.For<ILogger<WatchlistMemberRegistrationDataJsonLoader>>();
            var loader = new WatchlistMemberRegistrationDataJsonLoader(loaderLogger);
            Manager = new WatchlistMemberRegistrationManager(logger, Repository, loader);

            RegistrationDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(RegistrationDir);
        }

        [Fact]
        public async Task WatchlistMemberRegistrationByMetadataFile_RelativePhotoPath()
        {
            Directory.SetCurrentDirectory(RegistrationDir);

            var registeredData = new List<RegisterWatchlistMemberData>();
            Repository.WhenForAnyArgs(r => r.RegisterAsync(null, null)).Do(info => registeredData.Add(info.Arg<RegisterWatchlistMemberData>()));

            const string photoFileName = "relativePhoto.jpeg";
            CreatePhotoFile(photoFileName);

            const string id = "120";
            var jsonFileContent = GetJsonMetadataFileContent(() => (id, new[] { photoFileName }));
            CreateDataFile(jsonFileContent);

            var requestParams = new RegisterRequestParams(20, 200, 4000, "cpu", "cpu", new[] { "Wl" });

            var result = await Manager.RegisterWatchlistMembersFromDirByMetadataFileAsync(RegistrationDir, requestParams, 1, CancellationToken.None);
            Assert.Empty(result.Failures);

            Assert.Single(registeredData);
            var registered = registeredData.Single();
            Assert.Equal(id, registered.Id);
            Assert.Single(registered.ImageData);
            var imageData = registered.ImageData.Single();
            Assert.Equal(photoFileName, Encoding.UTF8.GetString(imageData.Data));
        }

        [Fact]
        public async Task WatchlistMemberRegistrationByMetadataFile_AbsolutePhotoPath()
        {
            var registeredData = new List<RegisterWatchlistMemberData>();
            Repository.WhenForAnyArgs(r => r.RegisterAsync(null, null)).Do(info => registeredData.Add(info.Arg<RegisterWatchlistMemberData>()));

            const string photoFileName = "absolutePhoto.jpeg";
            var absolutePathToPhotoFile = CreatePhotoFile(photoFileName).Replace(@"\", @"\\");

            const string id = "12345";
            var jsonFileContent = GetJsonMetadataFileContent(() => (id, new[] { absolutePathToPhotoFile }));
            CreateDataFile(jsonFileContent);

            var requestParams = new RegisterRequestParams(20, 200, 4000, "cpu", "cpu", new[] { "Wl" });
            var result = await Manager.RegisterWatchlistMembersFromDirByMetadataFileAsync(RegistrationDir, requestParams, 1, CancellationToken.None);
            Assert.Empty(result.Failures);

            Assert.Single(registeredData);
            var registered = registeredData.Single();
            Assert.Equal(id, registered.Id);
            Assert.Single(registered.ImageData);
            var imageData = registered.ImageData.Single();
            Assert.Equal(photoFileName, Encoding.UTF8.GetString(imageData.Data));
        }

        [Fact]
        public async Task WatchlistMemberRegistrationByMetadataFile_Failures()
        {
            var registeredData = new List<RegisterWatchlistMemberData>();

            var jsonFileContent = GetJsonMetadataFileContent(
                () => ("1", new[] { CreatePhotoFile("photo1.jpeg") }),
                () => ("2", new[] { CreatePhotoFile("photo2.jpeg") }),
                () => ("3", new[] { CreatePhotoFile("photo3.jpeg") }));

            CreateDataFile(jsonFileContent);

            var ex = new TimeoutException("rpc timeout");

            Repository.RegisterAsync(Arg.Is<RegisterWatchlistMemberData>(r => r.Id == "2"), Arg.Any<Action<RegisterWatchlistMemberRequest>>())
                .Returns(Task.FromException<WatchlistMemberWithRelatedData>(ex));

            Repository.When(r => r.RegisterAsync(Arg.Is<RegisterWatchlistMemberData>(r => r.Id != "2"), Arg.Any<Action<RegisterWatchlistMemberRequest>>()))
                .Do(info => registeredData.Add(info.Arg<RegisterWatchlistMemberData>()));

            var requestParams = new RegisterRequestParams(20, 200, 4000, "cpu", "cpu", new[] { "Wl" });
            var result = await Manager.RegisterWatchlistMembersFromDirByMetadataFileAsync(RegistrationDir, requestParams, 1, CancellationToken.None);
            var failure = Assert.Single(result.Failures);

            // ReSharper disable once PossibleNullReferenceException
            Assert.Equal(ex, failure.Exception);
            Assert.EndsWith("photo2.jpeg", failure.PhotoPaths.Single());

            Assert.Equal(2, registeredData.Count);
        }

        private string CreatePhotoFile(string photoFileName)
        {
            string photoFileFullPath = Path.Combine(RegistrationDir, photoFileName);
            File.WriteAllBytes(photoFileFullPath, Encoding.UTF8.GetBytes(photoFileName));
            return photoFileFullPath;
        }

        private void CreateDataFile(string content)
        {
            string dataFileFullPath = Path.Combine(RegistrationDir, "extendedData.json");
            File.WriteAllText(dataFileFullPath, content, Encoding.UTF8);
        }

        private static string GetJsonMetadataFileContent(params Func<(string id, string[] PhotoFiles)>[] metadataInfoFunc)
        {
            var metadataObjArray = new List<ExpandoObject>();

            foreach (var metadataInfo in metadataInfoFunc)
            {
                var (id, photoFiles) = metadataInfo();

                dynamic wlMetadata = new ExpandoObject();

                wlMetadata.Id = id;
                wlMetadata.DisplayName = "Display name";
                wlMetadata.FullName = "Full name";
                wlMetadata.Note = "Example note";
                wlMetadata.Note = "1";
                wlMetadata.PhotoFiles = photoFiles;

                metadataObjArray.Add(wlMetadata);
            }

            return JsonConvert.SerializeObject(metadataObjArray, Formatting.Indented);
        }
    }
}
