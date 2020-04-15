using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "faces", Description = "Query Face entities")]
    public class FromFaces : From<Face>
    {
        public FromFaces(IQueryDataSelector<Face> selector) : base(selector)
        {
        }
    }
}