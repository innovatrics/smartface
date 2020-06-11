using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class IndividualODataSelector : ODataSelector<Individual>, IQueryDataSelector<Individual>
    {
        public IndividualODataSelector(Container container) : base(container.Individuals)
        {
        }
    }
}