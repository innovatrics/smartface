using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Impl
{
    public class RegisterWatchlistMemberExtendedJsonLoader
    {
        private ILogger<RegisterWatchlistMemberExtendedJsonLoader> Log { get; }

        public RegisterWatchlistMemberExtendedJsonLoader(ILogger<RegisterWatchlistMemberExtendedJsonLoader> log)
        {
            Log = log;
        }

        public RegisterWatchlistMemberExtended[] GetRegisterWatchlistMemberExtendedData(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ProcessingException($"Directory does not exists {path}");
            }

            var files = Directory.GetFiles(path, "*.json");
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
            var result = JsonConvert.DeserializeObject<RegisterWatchlistMemberExtended[]>(content);
            return result;
        }
    }
}
