using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core.Enums;
using SmartFace.ODataClient.SmartFace.WebApi.Models.Requests;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class ScopesRepository : IScopesRepository
    {
        private Container Container { get; }

        public ScopesRepository(Container container)
        {
            Container = container;
        }

        public ScopeModel Create()
        {
            var scopeData = ScopeData.CreateScopeData(ScopeType.Location);
            var scope = Container.Scopes.Create(scopeData);
            return new ScopeModel
            {
                Id = scope.Id
            };
        }
    }
}