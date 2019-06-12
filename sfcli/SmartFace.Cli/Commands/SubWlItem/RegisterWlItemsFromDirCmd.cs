using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistItem;

namespace SmartFace.Cli.Commands.SubWlItem
{
    [Command(Name = "registerFromDir", Description = "Register WlItem entities from photos in directory in format {wlitem_externalId}.(jpeg|jpg|png) ")]
    public class RegisterWlItemsFromDirCmd
    {
        private IWatchlistItemRegistrationManager Manager { get; }
        
        [Option("-w|--watchlistsExternalIds", "", CommandOptionType.MultipleValue)]
        public string[] WatchlistExternalIds { get; set; }
        
        [Option("-d|--dirToPhotos", "", CommandOptionType.SingleValue)]
        public string Directory { get; set; }

        [Option("-m|--metaDataFile", @"Use this option when you can provide single json file in selected directory with meta data for WlItem. In this case could be use any name for photo file
[
{
    ""ExternalId"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""file1.jpeg"", ""file2.jpeg""]
} 
]
", CommandOptionType.NoValue)]
        public bool UseMetaDataFile { get; set; }

        public RegisterWlItemsFromDirCmd(IWatchlistItemRegistrationManager manager)
        {
            Manager = manager;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (UseMetaDataFile)
            {
                Manager.RegisterWlItemsExtendedFromDir(Directory, WatchlistExternalIds);
            }
            else
            {
                Manager.RegisterWlItemsFromDir(Directory, WatchlistExternalIds);
            }
            return Constants.EXIT_CODE_OK;
        }
    }
}
