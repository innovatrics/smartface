using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistItem;

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
            Manager.RegisterWlItem(ExternalId, WatchlistExternalIds, Photos);
            return Constants.EXIT_CODE_OK;
        }

    }
}