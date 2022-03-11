using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Impl
{
    public class WatchlistMemberRegistrationDataJsonLoader
    {
        private ILogger<WatchlistMemberRegistrationDataJsonLoader> Log { get; }

        public WatchlistMemberRegistrationDataJsonLoader(ILogger<WatchlistMemberRegistrationDataJsonLoader> log)
        {
            Log = log;
        }

        public WatchlistMemberMetadata[] GetWatchlistMemberRegistrationData(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ProcessingException($"Directory does not exists {path}");
            }

            var files = Directory.GetFiles(path, "*.json", new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive
            });

            if (files.Length == 0)
            {
                throw new ProcessingException($"Selected directory does not contain any json file with RegisterWatchlistMemberExtended data [{path}]");
            }

            var file = files.First();
            if (files.Length > 1)
            {
                Log.LogWarning($"Selected directory contains more than one json file. Using first one [{file}]");
            }

            var content = File.ReadAllText(file);
            var result = JsonConvert.DeserializeObject<WatchlistMemberMetadata[]>(content);

            foreach (var metadata in result)
            {
                metadata.PhotoFiles = metadata.PhotoFiles.Select(s => NormalizePhotoPath(path, s)).ToArray();
            }

            return result;
        }

        private string NormalizePhotoPath(string directory, string photoFile)
        {
            if (string.IsNullOrEmpty(photoFile))
            {
                return photoFile;
            }

            if (Path.IsPathRooted(photoFile))
            {
                return photoFile;
            }
            
            var normalizedPhotoPath = Path.Combine(directory, photoFile);

            Log.LogInformation($"PhotoFile {photoFile} is relative, normalize to rooted path : {normalizedPhotoPath}");

            return normalizedPhotoPath;
        }
    }
}
