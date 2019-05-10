using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

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