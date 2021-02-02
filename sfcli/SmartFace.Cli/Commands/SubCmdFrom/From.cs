using System.Collections;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.DataSelector;

namespace SmartFace.Cli.Commands.SubCmdFrom
{

    [Command(Description = "From clause is the source of a rowset to be operated upon"),
     Subcommand(typeof(ExportImage))]
    public abstract class From<T> : IFromCmd
    {
        [Option("-c|--condition", "Return only those elements for which the expression is true. Expression for the query is in C# language. Use name of entity in singular form as base object. E.g. ...from faces --condition \"face.Id == 42\"", CommandOptionType.SingleValue)]
        public string Condition { get; } = string.Empty;

        [Option("-m|--map", "Projects each element of a sequence into a new form. Use name of entity in singular form as base object. E.g. ...from faces --map \"{ Id = face.Id, TrackletId = face.TrackletId, FormattedDt = face.CreatedAt.ToString(\"yyyyMMdd_HHmmss.ffff\"), face.ImageUrl}", CommandOptionType.SingleValue)]
        public string Map { get; } = string.Empty;

        [Option("-l|--linq", "Linq query (cannot be combined with --map option). E.g. ...from faces --linq Select(f => new { f.Id, f.TrackletId, FormattedDt = f.CreatedAt.ToString(\"yyyyMMdd_HHmmss.ffff\"), f.ImageUrl})", CommandOptionType.SingleValue)]
        public string Linq { get; } = string.Empty;

        [Option("-e|--expand", "The property for expand. E.g. ....from faces --expand Tracklet", CommandOptionType.SingleValue)]
        public string ExpandProperty { get; } = string.Empty;

        protected QueryCmd Parent { get; set; }

        private IQueryDataSelector<T> DataSelector { get; }

        protected From(IQueryDataSelector<T> dataSelector)
        {
            DataSelector = dataSelector;
        }

        public virtual IEnumerable Execute(IConsole console)
        {
            var entities = DataSelector.Execute(Condition, ExpandProperty, Linq, Map);
            return entities;
        }

        protected virtual void OnExecute(IConsole console)
        {
            IEnumerable entities;
            try
            {
                entities = Execute(console);
            }
            catch (ProcessingException e)
            {
                throw new ProcessingException($"{e.Message} Use argument {Constants.ARGUMENT_URL_ODATA} or set environment variable {Constants.ENVIRONMENT_URL_ODATA}.");
            }
            Parent.Execute(console, entities);
        }
    }
}