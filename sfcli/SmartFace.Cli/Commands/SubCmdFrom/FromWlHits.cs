using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "wlhits", Description = "Query WlHit entities")]
    public class FromWlHits : From<WlHit>
    {
        public FromWlHits(IQueryDataSelector<WlHit> selector) : base(selector)
        {
        }
    }
}