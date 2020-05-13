using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ManagementApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Impl
{
    public class WatchlistMemberRegistrationManager : IWatchlistMemberRegistrationManager
    {
        private ILogger<WatchlistMemberRegistrationManager> Log { get; }

        private IWatchlistMembersRepository Repository { get; }

        private RegisterWatchlistMemberExtendedJsonLoader Loader { get; }

        public const string FILE_PATTERN = @"^([^.]+)\.([jJ][pP][eE]?[gG]|[pP][nN][gG])$";

        public WatchlistMemberRegistrationManager(ILogger<WatchlistMemberRegistrationManager> log, IWatchlistMembersRepository repository, RegisterWatchlistMemberExtendedJsonLoader loader)
        {
            Log = log;
            Repository = repository;
            Loader = loader;
        }

        public async Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(RegisterWatchlistMemberExtended registerWatchlistMemberExtended)
        {
            var data = new RegisterWatchlistMemberData
            {
                ExternalId = registerWatchlistMemberExtended.ExternalId,
                DisplayName = registerWatchlistMemberExtended.DisplayName,
                FullName = registerWatchlistMemberExtended.FullName,
                Note = registerWatchlistMemberExtended.Note,
                WatchlistExternalIds = registerWatchlistMemberExtended.WatchlistExternalIds
            };

            registerWatchlistMemberExtended.PhotoFiles.ToList().ForEach(pathToPhotoFile => data.ImageData.Add(new RegisterWatchlistMemberImageData
            {
                Data = File.ReadAllBytes(pathToPhotoFile),
                MIME = pathToPhotoFile.ToLower().EndsWith($".{Constants.PNG}") ? Constants.PNG_MIME_TYPE : Constants.JPEG_MIME_TYPE
            }));

            var result = await Repository.RegisterAsync(data);
            Log.LogInformation($"WatchlistMember registered. [{data.ExternalId}]");

            return result;
        }

        public Task RegisterWatchlistMembersFromDirAsync(string directory, string[] watchlistExternalIds,
            int maxDegreeOfParallelism)
        {
            if (!Directory.Exists(directory))
            {
                throw new ProcessingException($"Directory does not exists {directory}");
            }

            var groupedPhotos = GetPhotosGroupedByExternalId(directory);

            var extendedData = new List<RegisterWatchlistMemberExtended>();

            foreach (var group in groupedPhotos)
            {
                var externalId = group.Key;
                var photoPaths = group.Select(photoWithExternalId => photoWithExternalId.PhotoPath).ToArray();
                var registerWatchlistMemberExtended = new RegisterWatchlistMemberExtended
                {
                    ExternalId = externalId,
                    PhotoFiles = photoPaths,
                    WatchlistExternalIds = watchlistExternalIds
                };
                extendedData.Add(registerWatchlistMemberExtended);
            }

            var (sourceBlock, destinationBlock) = CreateProcessingBlocks(maxDegreeOfParallelism);

            PostDataToSourceBlock(extendedData, sourceBlock);

            return WaitForRegistrationCompletionAsync(sourceBlock, destinationBlock);
        }

        public Task RegisterWatchlistMembersExtendedFromDirAsync(string directory, string[] watchlistExternalIds,
            int maxDegreeOfParallelism)
        {
            var extendedData = Loader.GetRegisterWatchlistMemberExtendedData(directory);
            extendedData.ToList().ForEach(data => data.WatchlistExternalIds = watchlistExternalIds);

            Directory.SetCurrentDirectory(directory);
            
            var (sourceBlock, destinationBlock) = CreateProcessingBlocks(maxDegreeOfParallelism);

            PostDataToSourceBlock(extendedData, sourceBlock);

            return WaitForRegistrationCompletionAsync(sourceBlock, destinationBlock);
        }

        private static IEnumerable<IGrouping<string, PhotoWithExternalId>> GetPhotosGroupedByExternalId(string directory)
        {
            var files = Directory.GetFiles(directory);
            var validPhotos = new List<PhotoWithExternalId>();
            foreach (var file in files)
            {
                if (TryGetExternalIdFromPhotoFile(file, out string externalId))
                {
                    var photoWithId = new PhotoWithExternalId(externalId, file);
                    validPhotos.Add(photoWithId);
                }
            }

            var groupedPhotos = validPhotos.GroupBy(p => p.ExternalId);
            return groupedPhotos;
        }

        private static bool TryGetExternalIdFromPhotoFile(string photoPath, out string eid)
        {
            eid = string.Empty;
            bool isValid = false;
            Regex regex = new Regex(FILE_PATTERN);
            var file = Path.GetFileName(photoPath);
            Match match = regex.Match(file);

            if (match.Success)
            {
                isValid = true;
                eid = match.Groups[1].Value;
            }

            return isValid;
        }

        private static Task WaitForRegistrationCompletionAsync(
            TransformBlock<RegisterWatchlistMemberExtended, WatchlistMemberWithRelatedData> sourceBlock,
            ActionBlock<WatchlistMemberWithRelatedData> destinationBlock)
        {
            sourceBlock.Complete();
            return destinationBlock.Completion;
        }

        private void PostDataToSourceBlock(IEnumerable<RegisterWatchlistMemberExtended> extendedData,
            TransformBlock<RegisterWatchlistMemberExtended, WatchlistMemberWithRelatedData> sourceBlock)
        {
            foreach (var registerWatchlistMemberExtended in extendedData)
            {
                var posted = sourceBlock.Post(registerWatchlistMemberExtended);
                if (!posted)
                {
                    Log.LogError(
                        $"Unable to process member with external id [{registerWatchlistMemberExtended.ExternalId}]");
                }
            }
        }

        private (TransformBlock<RegisterWatchlistMemberExtended, WatchlistMemberWithRelatedData> sourceBlock,
            ActionBlock<WatchlistMemberWithRelatedData> destinationBlock) CreateProcessingBlocks(
                int maxDegreeOfParallelism)
        {
            var transformBlock = new TransformBlock<RegisterWatchlistMemberExtended, WatchlistMemberWithRelatedData>(
                async data =>
                {
                    try
                    {
                        return await RegisterWatchlistMemberAsync(data);
                    }
                    catch (Exception e)
                    {
                        Log.LogError($"Register member with externalId [{data.ExternalId}] failed", e);
                        return null;
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    EnsureOrdered = true,
                    MaxDegreeOfParallelism = maxDegreeOfParallelism

                });

            var actionBlock = new ActionBlock<WatchlistMemberWithRelatedData>(
                data => Log.LogInformation(JsonConvert.SerializeObject(data, Formatting.Indented)),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });

            transformBlock.LinkTo(actionBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            return (transformBlock, actionBlock);
        }
    }
}
