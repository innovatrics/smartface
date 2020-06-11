using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class WatchlistODataSelector : ODataSelector<Watchlist>, IQueryDataSelector<Watchlist>
    {
        public WatchlistODataSelector(Container container) : base(container.Watchlists)
        {
        }
    }
}