using System;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "sfcli", Description = "CLI for SmartFace instance")]
    public class BasicArgumentSolverCmd : IApiDefinition
    {
        [Option("-u|--url","SmartFace API Url (e.g. \"http://smartfaceserver:8099\")", CommandOptionType.SingleValue)]
        protected string Url { get; set; } = "http://localhost:8099";

        public int ZeroMqPort { get; } = 2406;

        public string ApiUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Url))
                {
                    return Environment.GetEnvironmentVariable(Constants.EnvironmentUrl);
                }

                return Url;
            }
        }

        public string ODataUrl => $"{ApiUrl.TrimEnd('/')}/odata";

        public string Host
        {
            get
            {
                Uri uri = new Uri(ApiUrl);
                return uri.Host;
            }
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            return Constants.EXIT_CODE_OK;
        }
    }
}
