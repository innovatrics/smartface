using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class IndividualODataSelector : ODataSelector<Identity>, IQueryDataSelector<Identity>
    {
        public IndividualODataSelector(Container container) : base(container.Identities)
        {
        }
    }
}