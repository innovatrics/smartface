using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistMember;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Commands.SubWatchlistMember
{
    [Command(Name = "register", Description = "Register single watchlist member")]
    public class RegisterWatchlistMemberCmd
    {
        private IWatchlistMemberRegistrationManager Manager { get; }

        [Required]
        [Option("-e|--externalId", "", CommandOptionType.SingleValue)]
        public string ExternalId { get; set; }

        [Required]
        [Option("-w|--watchlistsExternalIds", "", CommandOptionType.MultipleValue)]
        public string[] WatchlistExternalIds { get; set; }

        [RegistrationImgExtensionValidator]
        [FileExists]
        [Option("-p|--photos <FILE>", "", CommandOptionType.MultipleValue)]
        public string[] Photos { get; set; }

        public RegisterWatchlistMemberCmd(IWatchlistMemberRegistrationManager manager)
        {
            Manager = manager;
        }

        protected virtual async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
        {
            var data = new RegisterWatchlistMemberExtended
            {
                ExternalId = ExternalId,
                PhotoFiles = Photos,
                WatchlistExternalIds = WatchlistExternalIds
            };
            await Manager.RegisterWatchlistMemberAsync(data);
            return Constants.EXIT_CODE_OK;
        }

    }
}