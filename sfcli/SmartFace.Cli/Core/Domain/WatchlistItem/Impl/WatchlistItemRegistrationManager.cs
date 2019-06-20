using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.Utils;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain.WatchlistItem.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistItem.Impl
{
    public class WatchlistItemRegistrationManager : IWatchlistItemRegistrationManager
    {
        private ILogger<WatchlistItemRegistrationManager> Log { get; }

        private IWlItemsRepository Repository { get; }

        private RegisterWlItemExtendedJsonLoader Loader { get; }

        public const string FILE_PATTERN = @"^([^.]+)\.([jJ][pP][eE]?[gG]|[pP][nN][gG])$";

        public WatchlistItemRegistrationManager(ILogger<WatchlistItemRegistrationManager> log, IWlItemsRepository repository, RegisterWlItemExtendedJsonLoader loader)
        {
            Log = log;
            Repository = repository;
            Loader = loader;
        }

        public void RegisterWlItem(RegisterWlItemExtended registerWlItemExtended)
        {
            var data = new RegisterWlItemData
            {
                ExternalId = registerWlItemExtended.ExternalId,
                DisplayName = registerWlItemExtended.DisplayName,
                FullName = registerWlItemExtended.FullName,
                Note = registerWlItemExtended.Note,
                WatchlistExternalIds = registerWlItemExtended.WatchlistExternalIds
            };

            registerWlItemExtended.PhotoFiles.ToList().ForEach(pathToPhotoFile => data.ImageData.Add(new RegisterWlItemImageData
            {
                Data = File.ReadAllBytes(pathToPhotoFile),
                MIME = pathToPhotoFile.ToLower().EndsWith($".{Constants.PNG}") ? Constants.PNG_MIME_TYPE : Constants.JPEG_MIME_TYPE
            }));

            Repository.Register(data);

            Log.LogInformation($"WlItem registered. [{data.ExternalId}]");
        }

        public void RegisterWlItemsFromDir(string directory, string[] watchlistExternalIds, int maxDegreeOfParallelism)
        {
            if (!Directory.Exists(directory))
            {
                throw new ProcessingException($"Directory does not exists {directory}");
            }

            var groupedPhotos = GetPhotosGroupedByExternalId(directory);

            var extendedData = new List<RegisterWlItemExtended>();

            foreach (var group in groupedPhotos)
            {
                var externalId = group.Key;
                var photoPaths = group.Select(photoWithExternalId => photoWithExternalId.PhotoPath).ToArray();
                var registerWlItemExtended = new RegisterWlItemExtended
                {
                    ExternalId = externalId,
                    PhotoFiles = photoPaths,
                    WatchlistExternalIds = watchlistExternalIds
                };
                extendedData.Add(registerWlItemExtended);
            }

            var actionBlock = CreateRegisterActionBlock(maxDegreeOfParallelism);

            PostDataToActionBlock(extendedData, actionBlock);

            WaitForRegistrationCompletion(actionBlock);
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

        public void RegisterWlItemsExtendedFromDir(string directory, string[] watchlistExternalIds, int maxDegreeOfParallelism)
        {
            var extendedData = Loader.GetRegisterWlItemExtendedData(directory);
            extendedData.ToList().ForEach(data => data.WatchlistExternalIds = watchlistExternalIds);

            Directory.SetCurrentDirectory(directory);

            var actionBlock = CreateRegisterActionBlock(maxDegreeOfParallelism);

            PostDataToActionBlock(extendedData, actionBlock);

            WaitForRegistrationCompletion(actionBlock);
        }

        private static void WaitForRegistrationCompletion(ActionBlock<RegisterWlItemExtended> actionBlock)
        {
            actionBlock.Complete();
            actionBlock.Completion.AwaitSync();
        }

        private void PostDataToActionBlock(IEnumerable<RegisterWlItemExtended> extendedData, ActionBlock<RegisterWlItemExtended> actionBlock)
        {
            foreach (var registerWlItemExtended in extendedData)
            {
                var posted = actionBlock.Post(registerWlItemExtended);
                if (!posted)
                {
                    Log.LogError($"Unable to process item with external id [{registerWlItemExtended.ExternalId}]");
                }
            }
        }

        private ActionBlock<RegisterWlItemExtended> CreateRegisterActionBlock(int maxDegreeOfParallelism)
        {
            var actionBlock = new ActionBlock<RegisterWlItemExtended>(data =>
                {
                    try
                    {
                        RegisterWlItem(data);
                    }
                    catch (Exception e)
                    {
                        Log.LogError($"Register item with externalId [{data.ExternalId}] failed", e);
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
