using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.OData.Client;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class ODataSelector<TEntity>
    {
        private static readonly Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(ass => !ass.IsDynamic).ToArray(); // (_:_)

        private DataServiceQuery<TEntity> DefaultQuery { get; }

        protected ODataSelector(DataServiceQuery<TEntity> defaultQuery)
        {
            DefaultQuery = defaultQuery;
        }

        public virtual IEnumerable Execute(string condition, string expandProperty, string linq, string linqSelectExpression)
        {
            if (DefaultQuery.Context.BaseUri == null)
            {
                throw new ProcessingException($"API url was not set.");
            }

            DataServiceQuery<TEntity> query = DefaultQuery;
            if (!string.IsNullOrEmpty(condition))
            {
                query = Where(query, condition);
            }

            if (!string.IsNullOrEmpty(expandProperty))
            {
                query = ExpandQuery(query, expandProperty);
            }

            var task = Task.Run(async () =>
            {
                var getAllPagesAsync = query.GetType().GetMethods()
                    .Single(m => m.Name == nameof(query.GetAllPagesAsync));
                var res = await (dynamic)getAllPagesAsync.Invoke(query, null);
                return res;
            });
            IEnumerable entities = task.GetAwaiter().GetResult();

            entities = LocalQuery<TEntity>(linq, linqSelectExpression, entities);

            return entities;
        }

        protected static IEnumerable LocalQuery<T>(string linq, string linqSelectExpression, IEnumerable entities)
        {
            if (!string.IsNullOrEmpty(linq))
            {
                if (!string.IsNullOrEmpty(linqSelectExpression))
                {
                    throw new ProcessingException("Map properties is not allowed when linq query is executed.");
                }
                entities = Linq<T>(entities, linq);
            }

            if (!string.IsNullOrEmpty(linqSelectExpression))
            {
                entities = LinqSelect<T>(entities, linqSelectExpression);
            }

            return entities;
        }

        protected virtual DataServiceQuery<TEntity> ExpandQuery(DataServiceQuery<TEntity> query, string expandProperty)
        {
            return query.Expand(expandProperty);
        }

        #region private

        private static DataServiceQuery<T> Where<T>(DataServiceQuery<T> query, string where)
        {
            var globals = new Globals<T> { Query = query };
            var scriptOptions = GetScriptOptions();
            var entity = typeof(T).Name;

            var whereExpression = $".Where({entity.ToLower()} => {@where})";
            string expression = $"((DataServiceQuery<{entity}>)(Query as DataServiceQuery<{entity}>){whereExpression})";
            query = CSharpScript.RunAsync<DataServiceQuery<T>>(expression, scriptOptions, globals).GetAwaiter().GetResult().ReturnValue;
            return query;
        }

        private static IEnumerable LinqSelect<T>(IEnumerable entities, string map)
        {
            var entity = typeof(T).Name;
            return Linq<T>(entities, $"Select({entity.ToLower()} => new {map})");
        }

        protected static IEnumerable Linq<T>(IEnumerable entities, string linqExpression)
        {
            var globals = new LinqGlobals { Entities = entities };
            var scriptOptions = GetScriptOptions();
            var entity = typeof(T).Name;

            string expression = $"((Entities as IEnumerable<{entity}>).{linqExpression})";
            var result = CSharpScript.RunAsync<IEnumerable>(expression, scriptOptions, globals).GetAwaiter().GetResult().ReturnValue;
            return result;
        }

        private static ScriptOptions GetScriptOptions()
        {
            var scriptOptions = ScriptOptions.Default.AddImports(new[]
            {
                "System",
                "System.Linq",
                "System.Linq.Queryable",
                "System.Collections.Generic",
                "Microsoft.OData.Client",
                "SmartFace.Cli.Core.Domain.DataSelector",
                "SmartFace.Cli.Core.Domain.DataSelector.Impl",
                "SmartFace.ODataClient.SmartFace.Data.Models.Core"
            }).AddReferences(Assemblies);
            return scriptOptions;
        }

        #endregion

    }
    #region Globals

    public class Globals<TT>
    {
        public DataServiceQuery<TT> Query;
    }

    public class LinqGlobals
    {
        public IEnumerable Entities;
    }

    #endregion

}