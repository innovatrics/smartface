using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Commands.SubWatchlistMember;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "watchlistmember", Description = "Operations with watchlist member"),
    Subcommand(typeof(RegisterWatchlistMemberCmd)),
    Subcommand(typeof(RegisterWatchlistMembersFromDirCmd)),
    ]
    public class WatchlistMemberCmd
    {
        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.HELP_SPECIFY_SUB_CMD);
            return Constants.EXIT_CODE_OK;
        }
    }
}
