using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistItem;

namespace SmartFace.Cli.Commands.SubWlItem
{
    [Command(Name = "registerFromDir", Description = "Register WlItem entities from photos in directory in format {wlitem_externalId}_*.(jpeg|jpg|png) ")]
    public class RegisterWlItemsFromDirCmd
    {
        private IWatchlistItemRegistrationManager Manager { get; }
        
        [Option("-w|--watchlistsExternalIds", "", CommandOptionType.MultipleValue)]
        public string[] WatchlistExternalIds { get; set; }
        
        [Option("-d|--dirToPhotos", "", CommandOptionType.SingleValue)]
        public string Directory { get; set; }

        public RegisterWlItemsFromDirCmd(IWatchlistItemRegistrationManager manager)
        {
            Manager = manager;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            Manager.RegisterWlItemsFromDir(Directory, WatchlistExternalIds);
            return Constants.EXIT_CODE_OK;
        }
    }
}
