using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class FrameODataSelector : ODataSelector<Frame>, IQueryDataSelector<Frame>
    {
        public FrameODataSelector(Container container) : base(container.Frames)
        {
        }
    }
}