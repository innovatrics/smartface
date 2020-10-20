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

        public async Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(WatchlistMemberRegistrationData watchlistMemberRegistrationData)
        {
            var data = new RegisterWatchlistMemberData
            {
                Id = watchlistMemberRegistrationData.Id,
                DisplayName = watchlistMemberRegistrationData.DisplayName,
                FullName = watchlistMemberRegistrationData.FullName,
                Note = watchlistMemberRegistrationData.Note,
                WatchlistIds = watchlistMemberRegistrationData.WatchlistIds
            };

            watchlistMemberRegistrationData.PhotoFiles.ToList().ForEach(pathToPhotoFile => data.ImageData.Add(new RegisterWatchlistMemberImageData
            {
                Data = File.ReadAllBytes(pathToPhotoFile)
            }));

            var result = await _watchlistMembersRepository.RegisterAsync(data);
            _log.LogInformation($"WatchlistMember with Id {data.Id} registered.");

            return result;
        }

        public Task<RegistrationResult> RegisterWatchlistMembersFromDirAsync(string directory, string[] watchlistIds,
            int maxDegreeOfParallelism, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory does not exists {directory}");
            }

            var watchlistMemberRegistrationData = GetWlMemberRegistrationDataFromDirectory(directory, watchlistIds);
            return RegisterWatchlistMembersAsync(watchlistMemberRegistrationData, maxDegreeOfParallelism, cancellationToken);
        }

        public Task<RegistrationResult> RegisterWatchlistMembersFromDirByMetadataFileAsync(string directory, string[] watchlistIds,
            int maxDegreeOfParallelism, CancellationToken cancellationToken)
        {
            var registrationData = _jsonLoader.GetWatchlistMemberRegistrationData(directory);

            registrationData.ToList().ForEach(data => data.WatchlistIds = watchlistIds);

            return RegisterWatchlistMembersAsync(registrationData, maxDegreeOfParallelism, cancellationToken);
        }

        private async Task<RegistrationResult> RegisterWatchlistMembersAsync(IEnumerable<WatchlistMemberRegistrationData> watchlistMemberRegistrationData, int maxDegreeOfParallelism, CancellationToken cancellationToken)
        {
            var registrationResult = new RegistrationResult();

            var (sourceBlock, destinationBlock) = CreateProcessingBlocks((regData, ex) =>
            {
                var failure = new WatchlistMemberRegistrationFailure(regData.PhotoFiles, ex);
                registrationResult.Add(failure);

            }, maxDegreeOfParallelism);

            foreach (var memberRegistrationData in watchlistMemberRegistrationData)
            {
                var processed = await sourceBlock.SendAsync(memberRegistrationData, cancellationToken);

                if (!processed)
                {
                    _log.LogError($"Unable to process member with id [{memberRegistrationData.Id}]");
                }
            }

            sourceBlock.Complete();
            await destinationBlock.Completion;

            return registrationResult;
        }

        private (TransformBlock<WatchlistMemberRegistrationData, WatchlistMemberWithRelatedData> sourceBlock, ActionBlock<WatchlistMemberWithRelatedData> destinationBlock)
            CreateProcessingBlocks(Action<WatchlistMemberRegistrationData, Exception> errorAction, int maxDegreeOfParallelism)
        {
            var transformBlock = new TransformBlock<WatchlistMemberRegistrationData, WatchlistMemberWithRelatedData>(async registrationData =>
                {
                    try
                    {
                        return await RegisterWatchlistMemberAsync(registrationData);
                    }
                    catch (Exception e)
                    {
                        _log.LogError(e, $"Registration of watchlist member with Id [{registrationData.Id}] failed");
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
            }, x => x!= null);

            return (transformBlock, actionBlock);
        }

        private static IEnumerable<WatchlistMemberRegistrationData> GetWlMemberRegistrationDataFromDirectory(string directory, string[] watchlistIds)
        {
            var files = Directory.GetFiles(directory);

            var watchlistMemberPhotoPaths = new List<WatchlistMemberPhotoPath>();

            foreach (var file in files)
            {
                if (!TryGetWatchlistMemberIdFromFile(file, out var watchlistMemberId))
                {
                    continue;
                }

                var watchlistMemberPhotoPath = new WatchlistMemberPhotoPath(watchlistMemberId, file);
                watchlistMemberPhotoPaths.Add(watchlistMemberPhotoPath);
            }

            var groupedPhotos = watchlistMemberPhotoPaths.GroupBy(p => p.WatchlistMemberId).ToArray();

            var watchlistMemberRegistrationData = new List<WatchlistMemberRegistrationData>();

            foreach (var group in groupedPhotos)
            {
                var id = group.Key;
                var photoPaths = group.Select(photoWithId => photoWithId.PhotoPath).ToArray();

                var registerWatchlistMemberExtended = new WatchlistMemberRegistrationData
                {
                    Id = id,
                    PhotoFiles = photoPaths,
                    WatchlistIds = watchlistIds
                };

                watchlistMemberRegistrationData.Add(registerWatchlistMemberExtended);
            }

            return watchlistMemberRegistrationData;
        }

        private static bool TryGetWatchlistMemberIdFromFile(string filePath, out string wlMemberId)
        {
            wlMemberId = string.Empty;

            var regex = new Regex(PHOTO_FILE_NAME_WITH_EXT_PATTERN);
            var file = Path.GetFileName(filePath);
            var match = regex.Match(file);

            if (!match.Success)
            {
                return false;
            }

            wlMemberId = match.Groups[1].Value;
            return true;
        }
    }
}
