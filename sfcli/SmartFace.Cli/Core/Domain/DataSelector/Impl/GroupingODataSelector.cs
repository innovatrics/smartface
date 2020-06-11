using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class GroupingODataSelector : ODataSelector<GroupingMetadata>, IQueryDataSelector<GroupingMetadata>
    {
        public GroupingODataSelector(Container container) : base(container.GroupingMetadata)
        {
        }
    }
}