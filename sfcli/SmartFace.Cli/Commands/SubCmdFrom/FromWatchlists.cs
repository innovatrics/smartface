using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "watchlists", Description = "Query Watchlist entities")]
    public class FromWatchlists : From<Watchlist>
    {
        public FromWatchlists(IQueryDataSelector<Watchlist> selector) : base(selector)
        {
        }
    }
}