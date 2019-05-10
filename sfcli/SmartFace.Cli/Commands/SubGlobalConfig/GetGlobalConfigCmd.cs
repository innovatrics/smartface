using System;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SmartFace.Cli.Core.Domain.GlobalConfig;

namespace SmartFace.Cli.Commands.SubGlobalConfig
{
    [Command(Name = "get", Description = "Read properties of GlobalConfig")]
    public class GetGlobalConfigCmd
    {
        private IGlobalConfigRepository Repository { get; }

        public GetGlobalConfigCmd(IGlobalConfigRepository repository)
        {
            Repository = repository;
        }

        protected virtual void OnExecute(IConsole console)
        {
            var config = Repository.Get();
            var output = JsonConvert.SerializeObject(config, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                Formatting = Formatting.Indented
            });
            console.WriteLine(output);
        }
    }
}
