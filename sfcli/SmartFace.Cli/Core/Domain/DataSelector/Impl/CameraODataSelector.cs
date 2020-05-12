using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class CameraODataSelector : ODataSelector<Camera>, IQueryDataSelector<Camera>
    {
        public CameraODataSelector(Container container) : base(container.Cameras)
        {
        }
    }
}