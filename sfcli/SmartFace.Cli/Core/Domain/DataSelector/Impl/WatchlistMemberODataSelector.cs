using SmartFace.ODataClient.Default;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class WatchlistMemberODataSelector : ODataSelector<ODataClient.SmartFace.Domain.DataAccess.Models.Core.WatchlistMember>, IQueryDataSelector<ODataClient.SmartFace.Domain.DataAccess.Models.Core.WatchlistMember>
    {
        public WatchlistMemberODataSelector(Container container) : base(container.WatchlistMembers)
        {
        }
    }
}