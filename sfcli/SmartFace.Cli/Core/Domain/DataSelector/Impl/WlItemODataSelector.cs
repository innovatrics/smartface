using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class WlItemODataSelector : ODataSelector<WlItem>, IQueryDataSelector<WlItem>
    {
        public WlItemODataSelector(Container container) : base(container.WatchlistItems)
        {
        }
    }
}