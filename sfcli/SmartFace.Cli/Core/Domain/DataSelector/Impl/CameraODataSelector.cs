using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class CameraODataSelector : ODataSelector<Camera>, IQueryDataSelector<Camera>
    {
        public CameraODataSelector(Container container) : base(container.Cameras)
        {
        }
    }
}