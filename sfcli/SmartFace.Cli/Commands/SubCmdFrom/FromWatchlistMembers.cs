using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "watchlistmembers", Description = "Query WatchlistMember entities")]
    public class FromWatchlistMembers : From<WatchlistMember>
    {
        public FromWatchlistMembers(IQueryDataSelector<WatchlistMember> selector) : base(selector)
        {
        }
    }
}