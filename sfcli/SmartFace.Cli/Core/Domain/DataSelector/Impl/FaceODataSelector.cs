using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class FaceODataSelector : ODataSelector<Face>, IQueryDataSelector<Face>
    {
        public FaceODataSelector(Container container) : base(container.Faces)
        {
        }
    }
}