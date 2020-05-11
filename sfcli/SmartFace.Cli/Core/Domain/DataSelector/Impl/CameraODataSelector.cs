using SmartFace.ODataClient.Default;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class CameraODataSelector : ODataSelector<ODataClient.SmartFace.Domain.DataAccess.Models.Core.Camera>, IQueryDataSelector<ODataClient.SmartFace.Domain.DataAccess.Models.Core.Camera>
    {
        public CameraODataSelector(Container container) : base(container.Cameras)
        {
        }
    }
}