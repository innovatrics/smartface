using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Common;
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

        public void RegisterWlItem(RegisterWlItemExtended registerWlItemExtended, string[] watchlistExternalIds)
        {
            var data = new RegisterWlItemData
            {
                ExternalId = registerWlItemExtended.ExternalId,
                DisplayName = registerWlItemExtended.DisplayName,
                FullName = registerWlItemExtended.FullName,
                Note = registerWlItemExtended.Note
            };

            registerWlItemExtended.PhotoFiles.ToList().ForEach(pathToPhotoFile => data.ImageData.Add(new RegisterWlItemImageData
            {
                Data = File.ReadAllBytes(pathToPhotoFile),
                MIME = pathToPhotoFile.ToLower().EndsWith($".{Constants.PNG}") ? Constants.PNG_MIME_TYPE : Constants.JPEG_MIME_TYPE
            }));
            watchlistExternalIds.ToList().ForEach(w => data.WatchlistExternalIds.Add(w));

            Repository.Register(data);

            Log.LogInformation($"WlItem registered. [{data.ExternalId}]");
        }

        public void RegisterWlItemsFromDir(string directory, string[] watchlistExternalIds)
        {
            if (!Directory.Exists(directory))
            {
                throw new ProcessingException($"Directory does not exists {directory}");
            }

            var files = Directory.GetFiles(directory);
            var validPhotos = new List<PhotoWithExternalId>();
            foreach (var file in files)
            {
                var photoWithId = GetPhotoWithExternalId(file);
                if (photoWithId.IsValid)
                {
                    validPhotos.Add(photoWithId);
                }
            }

            var groupedPhotos = validPhotos.GroupBy(p => p.ExternalId);
            foreach (var group in groupedPhotos)
            {
                var externalId = group.Key;
                var photoPaths = group.Select(photoWithExternalId => photoWithExternalId.PhotoPath).ToArray();
                var registerWlItemExtended = new RegisterWlItemExtended
                {
                    ExternalId = externalId,
                    PhotoFiles = photoPaths
                };
                RegisterWlItem(registerWlItemExtended, watchlistExternalIds);
            }
        }

        private static PhotoWithExternalId GetPhotoWithExternalId(string photoPath)
        {
            string eid = string.Empty;
            bool isValid = false;
            Regex regex = new Regex(FILE_PATTERN);
            var file = Path.GetFileName(photoPath);
            Match match = regex.Match(file);

            if (match.Success)
            {
                isValid = true;
                eid = match.Groups[1].Value;
            }

            return new PhotoWithExternalId(isValid, eid, photoPath);
        }

        public void RegisterWlItemsExtendedFromDir(string directory, string[] watchlistExternalIds)
        {
            var extendedData = Loader.GetRegisterWlItemExtendedData(directory);

            Directory.SetCurrentDirectory(directory);

            foreach (var registerWlItemExtended in extendedData)
            {
                RegisterWlItem(registerWlItemExtended, watchlistExternalIds);
            }
        }
    }
}
