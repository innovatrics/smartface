using System;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "sfcli", Description = "CLI for SmartFace instance")]
    public class BasicArgumentSolverCmd : IApiDefinition
    {
        private string _host = "localhost";

        [Option("--host",
            "SmartFace host (e.g. \"smartfaceserver\"). Defaults to \"localhost\". Can be overridden by environment variable " +
            Constants.ENVIRONMENT_HOST,
            CommandOptionType.SingleValue)]
        public string Host
        {
            get => Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_HOST) ?? _host;
            set => _host = value;
        }

        public string Protocol { get; set; } = "http";

        [Option("-rp|--restPort",
            "Port under which the Rest API runs on the provided host. Defaults to 8098",
            CommandOptionType.SingleValue)]
        public int RestApiPort { get; set; } = 8098;

        [Option("-op|--odataPort",
            "Port under which the OData API runs on the provided host. Defaults to 8099",
            CommandOptionType.SingleValue)]
        public int ODataPort { get; set; } = 8099;

        public int ZeroMqPort => 2406;

        public string ApiUrl => $"{Protocol}://{Host}:{RestApiPort}";

        public string OdataBaseUrl => $"{Protocol}://{Host}:{ODataPort}";

        public string ODataUrl => $"{OdataBaseUrl}/odata";

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            return Constants.EXIT_CODE_OK;
        }
    }
}
