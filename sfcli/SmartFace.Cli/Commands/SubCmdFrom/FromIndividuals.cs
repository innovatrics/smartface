using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "individuals", Description = "Query Individual entities")]
    public class FromIndividuals : From<Individual>
    {
        public FromIndividuals(IQueryDataSelector<Individual> selector) : base(selector)
        {
        }
    }
}