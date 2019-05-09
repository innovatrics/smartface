using System;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Commands.SubWlItem;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "wlitem", Description = "Operations with watchlist item"),
    Subcommand(typeof(RegisterWlItemCmd)),
    Subcommand(typeof(RegisterWlItemsFromDirCmd)),
    ]
    public class WlItemCmd
    {
        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.HELP_SPECIFY_SUBCMD);
            return Constants.EXIT_CODE_OK;
        }
    }
}
