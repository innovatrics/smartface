using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ManagementApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFace.Cli.Commands.SubWatchlistMember;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Impl
{
    public class WatchlistMemberRegistrationManager : IWatchlistMemberRegistrationManager
    {
        private const string PHOTO_FILE_NAME_WITH_EXT_PATTERN = @"^([^.]+)\.([jJ][pP][eE]?[gG]|[pP][nN][gG])$";

        private readonly ILogger<WatchlistMemberRegistrationManager> _log;
        private readonly IWatchlistMembersRepository _watchlistMembersRepository;
        private readonly WatchlistMemberRegistrationDataJsonLoader _jsonLoader;

        public WatchlistMemberRegistrationManager(ILogger<WatchlistMemberRegistrationManager> log, IWatchlistMembersRepository repository, WatchlistMemberRegistrationDataJsonLoader loader)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _watchlistMembersRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _jsonLoader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        public async Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(WatchlistMemberRegisterData regData)
        {
            // prepare watchlist member data
            var wlMemberData = new RegisterWatchlistMemberData
            {
                Id = regData.WatchlistMemberMetadata.Id,
                DisplayName = regData.WatchlistMemberMetadata.DisplayName,
                FullName = regData.WatchlistMemberMetadata.FullName,
                Note = regData.WatchlistMemberMetadata.Note,
                WatchlistIds = regData.WatchlistMemberMetadata.WatchlistIds
            };

            regData.WatchlistMemberMetadata.PhotoFiles.ToList().ForEach(pathToPhotoFile => wlMemberData.ImageData.Add(new RegisterWatchlistMemberImageData
            {
                Data = File.ReadAllBytes(pathToPhotoFile)
            }));

            // update detection params
            var result = await _watchlistMembersRepository.RegisterAsync(wlMemberData, req =>
            {
                req.FaceDetectorResourceId = regData.RegisterRequestParams.FaceDetectionResourceId;
                req.TemplateGeneratorResourceId = regData.RegisterRequestParams.TemplateGeneratorResourceId;

                req.FaceDetectorConfig = new FaceDetectorConfig
                {
                    MinFaceSize = regData.RegisterRequestParams.MinFaceSize,
                    MaxFaceSize = regData.RegisterRequestParams.MaxFaceSize,
                    ConfidenceThreshold = regData.RegisterRequestParams.FaceConfidenceThreshold
                };
            });

            _log.LogInformation($"WatchlistMember with Id {wlMemberData.Id} registered.");

            return result;
        }

        public Task<RegistrationResult> RegisterWatchlistMembersFromDirAsync(
            string directory,
            RegisterRequestParams registerRequestParams,
            int maxDegreeOfParallelism,
            string fileNameToProperty,
            CancellationToken cancellationToken)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory does not exists {directory}");
            }

            var watchlistMemberRegistrationData = BuildWatchlistMemberRegisterData(GetFullPathToRegistrationDirectory(directory), fileNameToProperty, registerRequestParams.WatchlistIds, registerRequestParams);
            return RegisterWatchlistMembersAsync(watchlistMemberRegistrationData, maxDegreeOfParallelism, cancellationToken);
        }

        public Task<RegistrationResult> RegisterWatchlistMembersFromDirByMetadataFileAsync(
            string directory,
            RegisterRequestParams registerRequestParams,
            int maxDegreeOfParallelism,
            CancellationToken cancellationToken)
        {
            var wlmMembersMetadata = _jsonLoader.GetWatchlistMemberRegistrationData(GetFullPathToRegistrationDirectory(directory));

            wlmMembersMetadata.ToList().ForEach(data => data.WatchlistIds = registerRequestParams.WatchlistIds);

            var watchlistMemberRegistrationData = wlmMembersMetadata
                .Select(x => new WatchlistMemberRegisterData(x, registerRequestParams)).ToArray();

            return RegisterWatchlistMembersAsync(watchlistMemberRegistrationData, maxDegreeOfParallelism, cancellationToken);
        }

        private async Task<RegistrationResult> RegisterWatchlistMembersAsync(
            IEnumerable<WatchlistMemberRegisterData> watchlistMemberRegistrationData,
            int maxDegreeOfParallelism,
            CancellationToken cancellationToken)
        {
            var registrationResult = new RegistrationResult();

            var (sourceBlock, destinationBlock) = CreateProcessingBlocks((regData, ex) =>
            {
                var failure = new WatchlistMemberRegistrationFailure(regData.WatchlistMemberMetadata.PhotoFiles, ex);
                registrationResult.Add(failure);

            }, maxDegreeOfParallelism);

            var firstRequest = true;

            foreach (var memberRegistrationData in watchlistMemberRegistrationData)
            {
                var processed = await sourceBlock.SendAsync(memberRegistrationData, cancellationToken);

                // Delay sending of next request after first one is send to prevent cold start problem and possible parallel SQL watchlist creation conflict
                if (firstRequest)
                {
                    await Task.Delay(5000, cancellationToken);
                    firstRequest = false;
                }

                if (!processed)
                {
                    _log.LogError($"Unable to process member with id [{memberRegistrationData.WatchlistMemberMetadata.Id}]");
                }
            }

            sourceBlock.Complete();
            await destinationBlock.Completion;

            return registrationResult;
        }

        private (TransformBlock<WatchlistMemberRegisterData, WatchlistMemberWithRelatedData> sourceBlock, ActionBlock<WatchlistMemberWithRelatedData> destinationBlock)
            CreateProcessingBlocks(Action<WatchlistMemberRegisterData, Exception> errorAction, int maxDegreeOfParallelism)
        {
            var transformBlock = new TransformBlock<WatchlistMemberRegisterData, WatchlistMemberWithRelatedData>(async registrationData =>
                {
                    try
                    {
                        return await RegisterWatchlistMemberAsync(registrationData);
                    }
                    catch (Exception e)
                    {
                        _log.LogError(e, $"Registration of watchlist member with Id [{registrationData.WatchlistMemberMetadata.Id}] failed");
                        errorAction?.Invoke(registrationData, e);
                        return null;
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = 100,
                    EnsureOrdered = true,
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                });

            var actionBlock = new ActionBlock<WatchlistMemberWithRelatedData>(data =>
                {
                    _log.LogInformation(JsonConvert.SerializeObject(data, Formatting.Indented));
                },
                new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = 100,
                    MaxDegreeOfParallelism = 1
                });

            transformBlock.LinkTo(DataflowBlock.NullTarget<WatchlistMemberWithRelatedData>(), x => x == null);

            transformBlock.LinkTo(actionBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            }, x => x != null);

            return (transformBlock, actionBlock);
        }

        private string GetFullPathToRegistrationDirectory(string directory)
        {
            if (Path.IsPathRooted(directory))
            {
                _log.LogInformation($"Registration directory path : {directory}");
                return directory;
            }

            var workingDir = Directory.GetCurrentDirectory();

            _log.LogInformation($"Working directory : {workingDir}");

            var registrationDir = Path.Combine(workingDir, directory);

            _log.LogInformation($"Registration directory path : {registrationDir}");
            return registrationDir;
        }

        private IEnumerable<WatchlistMemberRegisterData> BuildWatchlistMemberRegisterData(
            string registrationDirectory,
            string fileNameToProperty,
            string[] watchlistIds,
            RegisterRequestParams requestParams)
        {
            var files = Directory.GetFiles(registrationDirectory);

            var watchlistMemberPhotoPaths = new List<WatchlistMemberPhotoPath>();

            foreach (var file in files)
            {
                if (!TryGetValidFileName(file, out var validFileName))
                {
                    continue;
                }

                switch (fileNameToProperty.ToLower())
                {
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fileNameToProperty));

                    case "id":
                        watchlistMemberPhotoPaths.Add(new WatchlistMemberPhotoPath() { WatchlistMemberId = validFileName, PhotoPath = file });
                        break;

                    case "name":
                    case "fullname":
                        watchlistMemberPhotoPaths.Add(new WatchlistMemberPhotoPath() { Name = validFileName, PhotoPath = file });
                        break;
                }
            }

            _log.LogInformation($"Found {watchlistMemberPhotoPaths.Count} images.");

            var groupedPhotos = watchlistMemberPhotoPaths.GroupBy(p => new { p.WatchlistMemberId, p.Name }).ToArray();

            var watchlistMemberRegistrationData = new List<WatchlistMemberRegisterData>();

            foreach (var group in groupedPhotos)
            {
                var id = group.Key.WatchlistMemberId ?? $"{Guid.NewGuid()}";
                var name = group.Key.Name;
                var photoPaths = group.Select(photoWithId => photoWithId.PhotoPath).ToArray();

                var watchlistMemberMetadata = new WatchlistMemberMetadata
                {
                    Id = id,
                    FullName = name,
                    DisplayName = name,
                    PhotoFiles = photoPaths,
                    WatchlistIds = watchlistIds
                };

                watchlistMemberRegistrationData.Add(new WatchlistMemberRegisterData(watchlistMemberMetadata, requestParams));
            }

            return watchlistMemberRegistrationData;
        }

        private static bool TryGetValidFileName(string filePath, out string validFileName)
        {
            validFileName = string.Empty;

            var regex = new Regex(PHOTO_FILE_NAME_WITH_EXT_PATTERN);
            var file = Path.GetFileName(filePath);
            var match = regex.Match(file);

            if (!match.Success)
            {
                return false;
            }

            validFileName = match.Groups[1].Value;
            return true;
        }
    }
}
