using System;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "sfcli", Description = "CLI for SmartFace instance")]
    public class BasicArgumentSolverCmd : IApiDefinition
    {
        private string _apiUrl = Constants.DEFAULT_URL_API;

        [Option(Constants.ARGUMENT_URL_API,
            "SmartFace REST API url (e.g. \"http://smartfaceserver:8098\"). Defaults to \"" + Constants.DEFAULT_URL_API +"\". Can be overridden by environment variable " +
            Constants.ENVIRONMENT_URL_API,
            CommandOptionType.SingleValue)]
        public string ApiUrl
        {
            get => Environment.GetEnvironmentVariable(Constants.DEFAULT_URL_API) ?? _apiUrl;
            set => _apiUrl = value;
        }
        
        private string _odataUrl = Constants.DEFAULT_URL_ODATA;

        [Option(Constants.ARGUMENT_URL_ODATA,
            "SmartFace ODATA API base url (e.g. \"http://smartfaceserver:8099\"). Defaults to \"" + Constants.DEFAULT_URL_ODATA + "\". Can be overridden by environment variable " +
            Constants.ENVIRONMENT_URL_ODATA,
            CommandOptionType.SingleValue)]
        public string OdataBaseUrl
        {
            get => Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_URL_ODATA) ?? _odataUrl;
            set => _odataUrl = value;
        }

        [Option("--zero-mq-host",
            "Hostname where SFBase service is available",
            CommandOptionType.SingleValue)]
        public string ZeroMqHost { get; } = "localhost";

        public int ZeroMqPort => 2406;

        public string ODataUrl => $"{OdataBaseUrl}/odata";

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            return Constants.EXIT_CODE_OK;
        }
    }
}
