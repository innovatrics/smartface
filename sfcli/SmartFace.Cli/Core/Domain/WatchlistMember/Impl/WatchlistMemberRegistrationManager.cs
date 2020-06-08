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
                Id = registerWatchlistMemberExtended.Id,
                DisplayName = registerWatchlistMemberExtended.DisplayName,
                FullName = registerWatchlistMemberExtended.FullName,
                Note = registerWatchlistMemberExtended.Note,
                WatchlistIds = registerWatchlistMemberExtended.WatchlistIds
            };

            registerWatchlistMemberExtended.PhotoFiles.ToList().ForEach(pathToPhotoFile => data.ImageData.Add(new RegisterWatchlistMemberImageData
            {
                Data = File.ReadAllBytes(pathToPhotoFile)
            }));

            var result = await Repository.RegisterAsync(data);
            Log.LogInformation($"WatchlistMember registered. [{data.Id}]");

            return result;
        }

        public Task RegisterWatchlistMembersFromDirAsync(string directory, string[] watchlistIds,
            int maxDegreeOfParallelism)
        {
            if (!Directory.Exists(directory))
            {
                throw new ProcessingException($"Directory does not exists {directory}");
            }

            var groupedPhotos = GetPhotosGroupedById(directory);

            var extendedData = new List<RegisterWatchlistMemberExtended>();

            foreach (var group in groupedPhotos)
            {
                var id = group.Key;
                var photoPaths = group.Select(photoWithId => photoWithId.PhotoPath).ToArray();
                var registerWatchlistMemberExtended = new RegisterWatchlistMemberExtended
                {
                    Id = id,
                    PhotoFiles = photoPaths,
                    WatchlistIds = watchlistIds
                };
                extendedData.Add(registerWatchlistMemberExtended);
            }

            var (sourceBlock, destinationBlock) = CreateProcessingBlocks(maxDegreeOfParallelism);

            PostDataToSourceBlock(extendedData, sourceBlock);

            return WaitForRegistrationCompletionAsync(sourceBlock, destinationBlock);
        }

        public Task RegisterWatchlistMembersExtendedFromDirAsync(string directory, string[] watchlistIds,
            int maxDegreeOfParallelism)
        {
            var extendedData = Loader.GetRegisterWatchlistMemberExtendedData(directory);
            extendedData.ToList().ForEach(data => data.WatchlistIds = watchlistIds);

            Directory.SetCurrentDirectory(directory);
            
            var (sourceBlock, destinationBlock) = CreateProcessingBlocks(maxDegreeOfParallelism);

            PostDataToSourceBlock(extendedData, sourceBlock);

            return WaitForRegistrationCompletionAsync(sourceBlock, destinationBlock);
        }

        private static IEnumerable<IGrouping<string, PhotoWithId>> GetPhotosGroupedById(string directory)
        {
            var files = Directory.GetFiles(directory);
            var validPhotos = new List<PhotoWithId>();
            foreach (var file in files)
            {
                if (TryGetIdFromPhotoFile(file, out string id))
                {
                    var photoWithId = new PhotoWithId(id, file);
                    validPhotos.Add(photoWithId);
                }
            }

            var groupedPhotos = validPhotos.GroupBy(p => p.Id);
            return groupedPhotos;
        }

        private static bool TryGetIdFromPhotoFile(string photoPath, out string id)
        {
            id = string.Empty;
            bool isValid = false;
            Regex regex = new Regex(FILE_PATTERN);
            var file = Path.GetFileName(photoPath);
            Match match = regex.Match(file);

            if (match.Success)
            {
                isValid = true;
                id = match.Groups[1].Value;
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
                        $"Unable to process member with id [{registerWatchlistMemberExtended.Id}]");
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
                        Log.LogError($"Register member with id [{data.Id}] failed", e);
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
