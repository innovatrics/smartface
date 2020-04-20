using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
        
    [Command(Name = "scopes", Description = "Query Scope entities")]
    public class FromScopes : From<Scope>
    {
        public FromScopes(IQueryDataSelector<Scope> selector) : base(selector)
        {
        }
    }
}