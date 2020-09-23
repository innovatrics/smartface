using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class PedestrianODataSelector : ODataSelector<Pedestrian>, IQueryDataSelector<Pedestrian>
    {
        public PedestrianODataSelector(Container container) : base(container.Pedestrians)
        {
        }
    }
}