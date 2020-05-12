using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
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

        public async Task RegisterWatchlistMemberAsync(RegisterWatchlistMemberExtended registerWatchlistMemberExtended)
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

            await Repository.RegisterAsync(data);
            Log.LogInformation($"WatchlistMember registered. [{data.ExternalId}]");
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

            var actionBlock = CreateRegisterActionBlock(maxDegreeOfParallelism);

            PostDataToActionBlock(extendedData, actionBlock);

            return WaitForRegistrationCompletionAsync(actionBlock);
        }

        public Task RegisterWatchlistMembersExtendedFromDirAsync(string directory, string[] watchlistExternalIds,
            int maxDegreeOfParallelism)
        {
            var extendedData = Loader.GetRegisterWatchlistMemberExtendedData(directory);
            extendedData.ToList().ForEach(data => data.WatchlistExternalIds = watchlistExternalIds);

            Directory.SetCurrentDirectory(directory);

            var actionBlock = CreateRegisterActionBlock(maxDegreeOfParallelism);

            PostDataToActionBlock(extendedData, actionBlock);

            return WaitForRegistrationCompletionAsync(actionBlock);
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

        private static Task WaitForRegistrationCompletionAsync(ActionBlock<RegisterWatchlistMemberExtended> actionBlock)
        {
            actionBlock.Complete();
            return actionBlock.Completion;
        }

        private void PostDataToActionBlock(IEnumerable<RegisterWatchlistMemberExtended> extendedData, ActionBlock<RegisterWatchlistMemberExtended> actionBlock)
        {
            foreach (var registerWatchlistMemberExtended in extendedData)
            {
                var posted = actionBlock.Post(registerWatchlistMemberExtended);
                if (!posted)
                {
                    Log.LogError($"Unable to process member with external id [{registerWatchlistMemberExtended.ExternalId}]");
                }
            }
        }

        private ActionBlock<RegisterWatchlistMemberExtended> CreateRegisterActionBlock(int maxDegreeOfParallelism)
        {
            var actionBlock = new ActionBlock<RegisterWatchlistMemberExtended>(async data =>
                {
                    try
                    {
                        await RegisterWatchlistMemberAsync(data);
                    }
                    catch (Exception e)
                    {
                        Log.LogError($"Register member with externalId [{data.ExternalId}] failed", e);
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    EnsureOrdered = true,
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                });
            return actionBlock;
        }
    }
}
