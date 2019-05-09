using System.Collections;

namespace SmartFace.Cli.Core.Domain.DataSelector
{
    public interface IQueryDataSelector<TEntity>
    {
        IEnumerable Execute(string condition, string expandProperty, string linq, string linqSelectExpression);
    }
}