using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistItem;
using SmartFace.Cli.Core.Domain.WatchlistItem.Impl;
using SmartFace.Cli.Core.Domain.WatchlistItem.Model;

namespace SmartFace.Cli.Commands.SubWlItem
{
    [Command(Name = "register", Description = "Register single watchlist item")]
    public class RegisterWlItemCmd
    {
        private IWatchlistItemRegistrationManager Manager { get; }

        [Option("-e|--externalId", "", CommandOptionType.SingleValue)]
        public string ExternalId { get; set; }

        [Option("-w|--watchlistsExternalIds", "", CommandOptionType.MultipleValue)]
        public string[] WatchlistExternalIds { get; set; }

        [RegistrationImgExtensionValidator]
        [FileExists]
        [Option("-p|--photos <FILE>", "", CommandOptionType.MultipleValue)]
        public string[] Photos { get; set; }

        public RegisterWlItemCmd(IWatchlistItemRegistrationManager manager)
        {
            Manager = manager;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            var data = new RegisterWlItemExtended
            {
                ExternalId = ExternalId,
                PhotoFiles = Photos,
            };
            Manager.RegisterWlItem(data, WatchlistExternalIds);
            return Constants.EXIT_CODE_OK;
        }

    }
}