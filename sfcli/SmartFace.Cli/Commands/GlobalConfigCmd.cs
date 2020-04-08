using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Commands.SubGlobalConfig;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "globalconfig", Description = "View or edit global smartface settings"),
     Subcommand(typeof(GetGlobalConfigCmd)),
     Subcommand(typeof(SetGlobalConfigCmd))]
    public class GlobalConfigCmd
    {
        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.HELP_SPECIFY_SUB_CMD);
            return Constants.EXIT_CODE_OK;
        }
    }
}
