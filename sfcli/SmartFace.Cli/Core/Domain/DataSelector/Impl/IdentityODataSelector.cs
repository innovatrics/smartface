using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class IdentityODataSelector : ODataSelector<Identity>, IQueryDataSelector<Identity>
    {
        public IdentityODataSelector(Container container) : base(container.Identities)
        {
        }
    }
}