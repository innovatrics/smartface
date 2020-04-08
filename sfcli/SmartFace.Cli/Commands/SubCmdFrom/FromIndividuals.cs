using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "individuals", Description = "Query Individual entities")]
    public class FromIndividuals : From<Identity>
    {
        public FromIndividuals(IQueryDataSelector<Identity> selector) : base(selector)
        {
        }
    }
}