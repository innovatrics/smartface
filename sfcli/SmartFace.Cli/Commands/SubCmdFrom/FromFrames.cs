using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "frames", Description = "Query Frame entities")]
    public class FromFrames : From<Photo>
    {
        public FromFrames(IQueryDataSelector<Photo> selector) : base(selector)
        {
        }
    }
}