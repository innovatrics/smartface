using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "watchlistitems", Description = "Query WatchlistItem entities")]
    public class FromWlItems : From<WlItem>
    {
        public FromWlItems(IQueryDataSelector<WlItem> selector) : base(selector)
        {
        }
    }
}