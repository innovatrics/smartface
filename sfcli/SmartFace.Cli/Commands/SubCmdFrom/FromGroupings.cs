using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "groupings", Description = "Query Camera entities")]
    public class FromGroupings : From<GroupingMetadata>
    {
        public FromGroupings(IQueryDataSelector<GroupingMetadata> selector) : base(selector)
        {
        }
    }
}