using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class PhotoODataSelector : ODataSelector<Photo>, IQueryDataSelector<Photo>
    {
        public PhotoODataSelector(Container container) : base(container.Photos)
        {
        }
    }
}