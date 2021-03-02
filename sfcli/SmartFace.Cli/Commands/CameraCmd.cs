using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Commands.SubCamera;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "camera", Description = "View or edit properties of camera configuration"),
    Subcommand(typeof(AddCameraCmd)),
    Subcommand(typeof(GetCameraCmd)),
    Subcommand(typeof(SetCameraCmd)),
    Subcommand(typeof(DeleteCameraCmd))
    ]
    public class CameraCmd
    {
        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.HELP_SPECIFY_SUB_CMD);
            return Constants.EXIT_CODE_OK;
        }
    }
}