using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using SmartFace.Cli.Common.Utils;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class MatchResultODataSelector : ODataSelector<MatchResult>, IQueryDataSelector<MatchResult>
    {
        private const string ALLOWED_EXPAND_PERSON = "Person";
        private const string ALLOWED_EXPAND_FACES = "Person($expand=Faces)";

        private Container Api { get; }

        public MatchResultODataSelector(Container api) : base(api.MatchResults)
        {
            Api = api;
        }

        protected override DataServiceQuery<MatchResult> ExpandQuery(DataServiceQuery<MatchResult> query, string expandProperty)
        {
            return query;
        }

        public override IEnumerable Execute(string condition, string expandProperty, string linq, string linqSelectExpression)
        {
            IEnumerable<MatchResult> entities;
            if (string.IsNullOrEmpty(expandProperty))
            {
                entities = (IEnumerable<MatchResult>)base.Execute(condition, expandProperty, linq, linqSelectExpression);
                return entities;
            }

            entities = (IEnumerable<MatchResult>)base.Execute(condition, expandProperty, string.Empty, string.Empty);

            var matchResultWithPersons = new ConcurrentBag<MatchResultWithPerson>();
            Parallel.ForEach(entities, (matchResult) =>
            {
                DataServiceQuery<Person> baseQuery;
                if (ALLOWED_EXPAND_PERSON.Equals(expandProperty, StringComparison.InvariantCultureIgnoreCase))
                {
                    baseQuery = Api.Persons;
                }
                else if (ALLOWED_EXPAND_FACES.Equals(expandProperty, StringComparison.InvariantCultureIgnoreCase))
                {
                    baseQuery = Api.Persons.Expand(p => p.Faces);
                }
                else
                {
                    throw new NotSupportedException();
                }

                var query = (DataServiceQuery<Person>)baseQuery.Where(p => p.Id == matchResult.PersonId);
                var person = query.ExecuteAsync().Result.SingleOrDefault();
                var matchResultWithPerson = new MatchResultWithPerson(person);
                matchResult.CopyProperties(matchResultWithPerson);
                matchResultWithPersons.Add(matchResultWithPerson);
            });

            var res = LocalQuery<MatchResultWithPerson>(linq, linqSelectExpression, matchResultWithPersons);
            return res;
        }
    }
}