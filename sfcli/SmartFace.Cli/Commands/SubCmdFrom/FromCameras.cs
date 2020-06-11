using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
        
    [Command(Name = "cameras", Description = "Query Camera entities")]
    public class FromCameras : From<Camera>
    {
        public FromCameras(IQueryDataSelector<Camera> selector) : base(selector)
        {
        }
    }
}