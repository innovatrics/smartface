using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class ScopeODataSelector : ODataSelector<Scope>, IQueryDataSelector<Scope>
    {
        public ScopeODataSelector(Container container) : base(container.Scopes)
        {
        }
    }
}