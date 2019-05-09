using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "identities", Description = "Query Identity entities")]
    public class FromIdentities : From<Identity>
    {
        public FromIdentities(IQueryDataSelector<Identity> selector) : base(selector)
        {
        }
    }
}