using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Commands.SubVideo;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "video", Description = "View or edit properties of video configuration"),
    Subcommand(typeof(AddVideoCmd)),
    Subcommand(typeof(GetVideoCmd)),
    Subcommand(typeof(SetVideoCmd)),
    ]
    public class VideoCmd
    {
        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.HELP_SPECIFY_SUBCMD);
            return Constants.EXIT_CODE_OK;
        }
    }
}