using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "photos", Description = "Query Photo entities")]
    public class FromPhotos : From<Photo>
    {
        public FromPhotos(IQueryDataSelector<Photo> selector) : base(selector)
        {
        }
    }
}