using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartFace.Cli.Commands.SubWatchlistMember;
using SmartFace.Cli.Core.Domain.WatchlistMember;
using Xunit;

namespace SmartFace.CliTests.SfCliTests.Commands
{
    public class RegisterWatchlistMembersFromDirCmdTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Test_Failed_Registrations_Photos_Copied_To_Export_Dir(bool useMetadataFile)
        {
            var sourceDir = Path.Combine(Path.GetTempPath(), "TMP_SF_CLI_REG_CMD_SOURCE_DIR");
            Directory.CreateDirectory(sourceDir);

            var exportDestDir = Path.Combine(Path.GetTempPath(), "TMP_SF_CLI_REG_CMD_EXPORT_DIR");
            Directory.CreateDirectory(exportDestDir);

            const string testFileName = "testPhoto.jpg";
            var testSourceFilePath = Path.Combine(sourceDir, testFileName);
            var fileStream = File.Create(testSourceFilePath);
            await fileStream.DisposeAsync();

            try
            {
                var regManager = Substitute.For<IWatchlistMemberRegistrationManager>();

                regManager.RegisterWatchlistMembersFromDirAsync(Arg.Any<string>(), Arg.Any<RegisterRequestParams>(), Arg.Any<int>(),
                        "id", Arg.Any<CancellationToken>())
                    .ReturnsForAnyArgs(new RegistrationResult().Add(new WatchlistMemberRegistrationFailure(new[] { testSourceFilePath }, new Exception("testEx"))));

                regManager.RegisterWatchlistMembersFromDirByMetadataFileAsync(Arg.Any<string>(), Arg.Any<RegisterRequestParams>(), Arg.Any<int>(),
                        Arg.Any<CancellationToken>())
                    .ReturnsForAnyArgs(new RegistrationResult().Add(new WatchlistMemberRegistrationFailure(new[] { testSourceFilePath }, new Exception("testEx"))));

                var logger = Substitute.For<ILogger<RegisterWatchlistMembersFromDirCmd>>();
                var cmd = new RegisterWatchlistMembersFromDirCmd(regManager, logger)
                {
                    WatchlistIds = new[] { "WL_ID_1" },
                    Directory = "abc",
                    MaxDegreeOfParallelism = 1,
                    UseMetaDataFile = useMetadataFile,
                    FailedRegistrationDir = exportDestDir
                };

                var consoleSub = Substitute.For<IConsole>();

                await cmd.OnExecuteAsync(new CommandLineApplication(consoleSub), consoleSub, CancellationToken.None);

                var copiedFilePath = Path.Combine(exportDestDir, testFileName);

                Assert.True(File.Exists(copiedFilePath));
                File.Delete(copiedFilePath);
            }
            finally
            {
                File.Delete(testSourceFilePath);
            }
        }
    }
}
