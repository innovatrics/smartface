using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "matchresults", Description = "Query MatchResult entities")]
    public class FromMatchResults : From<MatchResult>
    {
        public FromMatchResults(IQueryDataSelector<MatchResult> selector) : base(selector)
        {
        }
    }
}