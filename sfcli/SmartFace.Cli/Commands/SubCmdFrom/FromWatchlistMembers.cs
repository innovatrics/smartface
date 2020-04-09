using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "watchlistmembers", Description = "Query WatchlistMember entities")]
    public class FromWatchlistMembers : From<WlItem>
    {
        public FromWatchlistMembers(IQueryDataSelector<WlItem> selector) : base(selector)
        {
        }
    }
}