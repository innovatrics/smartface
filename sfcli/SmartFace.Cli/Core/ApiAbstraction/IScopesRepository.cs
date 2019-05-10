using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IScopesRepository
    {
        ScopeModel Create();
    }
}