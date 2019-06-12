using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistItem.Model;
using SmartFace.ODataClient.SmartFace.WebApi.Models.Responses;

namespace SmartFace.Cli.Core.Domain.WatchlistItem.Impl
{
    public class RegisterWlItemExtendedJsonLoader
    {
        private ILogger<RegisterWlItemExtendedJsonLoader> Log { get; }

        public RegisterWlItemExtendedJsonLoader(ILogger<RegisterWlItemExtendedJsonLoader> log)
        {
            Log = log;
        }

        public RegisterWlItemExtended[] GetRegisterWlItemExtendedData(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ProcessingException($"Directory does not exists {path}");
            }

            var files = Directory.GetFiles(path, "*.json");
            if (files.Length == 0)
            {
                throw new ProcessingException($"Selected directory does not contain any json file with RegisterWlItemExtended data [{path}]");
            }

            var file = files.First();
            if (files.Length > 1)
            {
                Log.LogWarning($"Selected directory contains more than one json file. Using first one [{file}]");
            }

            var content = File.ReadAllText(file);
            var result = JsonConvert.DeserializeObject<RegisterWlItemExtended[]>(content);
            return result;
        }
    }
}
