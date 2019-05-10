using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "groupings", Description = "Query Camera entities")]
    public class FromGroupings : From<Grouping>
    {
        public FromGroupings(IQueryDataSelector<Grouping> selector) : base(selector)
        {
        }
    }
}