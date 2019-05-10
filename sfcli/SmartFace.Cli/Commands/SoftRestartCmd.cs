using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.Utils;
using SmartFace.ODataClient.Action;
using SmartFace.ODataClient.Default;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "softRestart", Description = "Apply changes")]
    public class SoftRestartCmd
    {
        private Container Container { get; }

        public SoftRestartCmd(Container container)
        {
            Container = container;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            Container.Services.SoftRestart().ExecuteAsync().AsyncAwait();
            return Constants.EXIT_CODE_OK;
        }
    }
}
