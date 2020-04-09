using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class WatchlistMemberODataSelector : ODataSelector<WlItem>, IQueryDataSelector<WlItem>
    {
        public WatchlistMemberODataSelector(Container container) : base(container.WatchlistItems)
        {
        }
    }
}