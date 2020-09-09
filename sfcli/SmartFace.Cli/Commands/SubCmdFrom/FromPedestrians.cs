using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "pedestrians", Description = "Query Pedestrian entities")]
    public class FromPedestrians : From<Pedestrian>
    {
        public FromPedestrians(IQueryDataSelector<Pedestrian> dataSelector) : base(dataSelector)
        {
        }
    }
}