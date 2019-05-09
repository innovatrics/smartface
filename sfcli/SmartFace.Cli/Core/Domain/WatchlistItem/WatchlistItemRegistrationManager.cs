using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Core.Domain.WatchlistItem
{
    public class WatchlistItemRegistrationManager : IWatchlistItemRegistrationManager
    {
        private ILogger<WatchlistItemRegistrationManager> Log { get; }
        private IWlItemRepository Repository { get; }

        public const string FILE_PATTERN = @"^([^_.]+)(_[^\.]*)*\.([jJ][pP][eE]?[gG]|[pP][nN][gG])$";

        public WatchlistItemRegistrationManager(ILogger<WatchlistItemRegistrationManager> log, IWlItemRepository repository)
        {
            Log = log;
            Repository = repository;
        }

        public void RegisterWlItem(string wlItemExternalId, string[] watchlistExternalIds, string[] photoPaths)
        {
            var data = new RegisterWlItemData { ExternalId = wlItemExternalId };

            photoPaths.ToList().ForEach(p => data.ImageData.Add(new RegisterWlItemImageData
            {
                Data = File.ReadAllBytes(p),
                MIME = p.ToLower().EndsWith($".{Constants.PNG}") ? Constants.PNG_MIME_TYPE : Constants.JPEG_MIME_TYPE
            }));
            watchlistExternalIds.ToList().ForEach(w => data.WatchlistExternalIds.Add(w));

            Repository.Register(data);

            Log.LogInformation($"WlItem registered. [{wlItemExternalId}]");
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
                RegisterWlItem(externalId, watchlistExternalIds, photoPaths);
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

        private class PhotoWithExternalId
        {
            public bool IsValid { get; }

            public string ExternalId { get; }

            public string PhotoPath { get; }

            public PhotoWithExternalId(bool isValid, string externalId, string photoPath)
            {
                IsValid = isValid;
                ExternalId = externalId;
                PhotoPath = photoPath;
            }
        }

    }
}
