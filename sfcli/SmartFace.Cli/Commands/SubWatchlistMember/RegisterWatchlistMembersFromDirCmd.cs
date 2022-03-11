using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistMember;

namespace SmartFace.Cli.Commands.SubWatchlistMember
{
    [Command(Name = "registerFromDir", Description = "Register WatchlistMember entities from photos in directory in format {watchlistmember_id}.(jpeg|jpg|png) ")]
    public class RegisterWatchlistMembersFromDirCmd
    {
        private readonly ILogger<RegisterWatchlistMembersFromDirCmd> _logger;
        private readonly IWatchlistMemberRegistrationManager _registrationManager;

        [Option("--minFaceSize", "Default 25", CommandOptionType.SingleValue)]
        public int MinFaceSize { get; set; } = 25;

        [Option("--maxFaceSize", "Default 400", CommandOptionType.SingleValue)]
        public int MaxFaceSize { get; set; } = 400;

        [Option("--faceDetConfidenceThreshold", "Default 400", CommandOptionType.SingleValue)]
        public int FaceDetectionConfidenceThreshold { get; set; } = 400;

        [Option("--faceDetResourceId", "Default cpu", CommandOptionType.SingleValue)]
        public string FaceDetectionResourceId { get; set; } = "cpu";

        [Option("--templateGenResourceId", "Default cpu", CommandOptionType.SingleValue)]
        public string TemplateGeneratorResourceId { get; set; } = "cpu";

        [Required]
        [Option("-w|--watchlistIds", "Watchlist Id/s for enrollment", CommandOptionType.MultipleValue)]
        public string[] WatchlistIds { get; set; }

        [Required]
        [Option("-d|--dirToPhotos", "Directory path where photos for registration are expected.", CommandOptionType.SingleValue)]
        public string Directory { get; set; }

        [Option("-f|--failedRegistrationDirectory", "Path to directory name that will be created when some registrations failed. Photos of unsuccessful registrations will be copied here.", CommandOptionType.SingleValue)]
        public string FailedRegistrationDir { get; set; }

        [Option("-m|--metaDataFile", @"Use this option when you can provide single json file in selected directory with meta data for WatchlistMember. In this case could be use any name for photo file
[
{
    ""Id"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""file1.jpeg"", ""file2.jpeg""]
} 
]
", CommandOptionType.NoValue)]
        public bool UseMetaDataFile { get; set; }

        [Option("-p|--parallel", "Max degree of parallelism, default value is 1", CommandOptionType.SingleValue)]
        public int MaxDegreeOfParallelism { get; set; } = 1;

        [Option("--fileNameAs", "Use file name as ID or FullName", CommandOptionType.SingleValue)]
        public string FileNameToProperty { get; set; } = "id";

        public RegisterWatchlistMembersFromDirCmd(IWatchlistMemberRegistrationManager registrationManager, ILogger<RegisterWatchlistMembersFromDirCmd> logger)
        {
            _registrationManager = registrationManager;
            _logger = logger;
        }

        public virtual async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console, CancellationToken cancellationToken)
        {
            RegistrationResult registrationResult;

            var registerParams = new RegisterRequestParams(MinFaceSize, MaxFaceSize, FaceDetectionConfidenceThreshold, FaceDetectionResourceId, TemplateGeneratorResourceId, WatchlistIds);

            _logger.LogInformation("Detection parameters for register requests :");
            _logger.LogInformation(JsonConvert.SerializeObject(registerParams, Formatting.Indented));

            if (UseMetaDataFile)
            {
                _logger.LogWarning($"Registering watchlist members from directory {Directory} with metadata file.");
                registrationResult = await _registrationManager.RegisterWatchlistMembersFromDirByMetadataFileAsync(Directory, registerParams, MaxDegreeOfParallelism, cancellationToken);
            }
            else
            {
                _logger.LogWarning($"Registering watchlist members from directory {Directory}.");
                registrationResult = await _registrationManager.RegisterWatchlistMembersFromDirAsync(Directory, registerParams, MaxDegreeOfParallelism, FileNameToProperty, cancellationToken);
            }

            if (registrationResult.Failures.Any())
            {
                _logger.LogWarning($"{registrationResult.Failures} registrations failed.");

                var currentTimestampFormatted = DateTime.UtcNow.ToString("yyyy-MM-dd_hh-mm-ss");

                var failurePhotosDir = string.IsNullOrEmpty(FailedRegistrationDir)
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"SF_REG_FAILURE_{currentTimestampFormatted}")
                    : FailedRegistrationDir;

                System.IO.Directory.CreateDirectory(failurePhotosDir);

                _logger.LogWarning($"Directory for unsuccessful registration photos created at {failurePhotosDir}.");

                // Give OS some time to commit directory, CreateDirectory is kinda async
                Thread.Sleep(500);

                foreach (var photoPath in registrationResult.Failures.SelectMany(f => f.PhotoPaths))
                {
                    var destPath = Path.Combine(failurePhotosDir, Path.GetFileName(photoPath));
                    _logger.LogWarning($"Copying file from {photoPath} to {destPath}");
                    File.Copy(photoPath, destPath, true);
                }

                _logger.LogWarning($"Copying to {failurePhotosDir} done.");
            }
            else
            {
                _logger.LogInformation("Registration finished successfully without any errors.");
            }

            return Constants.EXIT_CODE_OK;
        }
    }
}
