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
    public class WlHitODataSelector : ODataSelector<WlHit>, IQueryDataSelector<WlHit>
    {
        private const string AllowedExpandPerson = "Person";
        private const string AllowedExpandFaces = "Person($expand=Faces)";

        private Container Api { get; }

        public WlHitODataSelector(Container api) : base(api.WlHits)
        {
            Api = api;
        }

        protected override DataServiceQuery<WlHit> ExpandQuery(DataServiceQuery<WlHit> query, string expandProperty)
        {
            return query;
        }

        public override IEnumerable Execute(string condition, string expandProperty, string linq, string linqSelectExpression)
        {
            IEnumerable<WlHit> entities;
            if (string.IsNullOrEmpty(expandProperty))
            {
                entities = (IEnumerable<WlHit>)base.Execute(condition, expandProperty, linq, linqSelectExpression);
                return entities;
            }

            entities = (IEnumerable<WlHit>)base.Execute(condition, expandProperty, string.Empty, string.Empty);

            var wlHitsWithFaces = new ConcurrentBag<WlHitWithPerson>();
            Parallel.ForEach(entities, (wlHit) =>
            {
                DataServiceQuery<Person> baseQuery;
                if (AllowedExpandPerson.Equals(expandProperty, StringComparison.InvariantCultureIgnoreCase))
                {
                    baseQuery = Api.Persons;
                }
                else if (AllowedExpandFaces.Equals(expandProperty, StringComparison.InvariantCultureIgnoreCase))
                {
                    baseQuery = Api.Persons.Expand(p => p.Faces);
                }
                else
                {
                    throw new NotSupportedException();
                }

                var query = (DataServiceQuery<Person>)baseQuery.Where(p => p.Id == wlHit.PersonId);
                var person = query.ExecuteAsync().Result.SingleOrDefault();
                var wlHitWithFaces = new WlHitWithPerson(person);
                wlHit.CopyProperties(wlHitWithFaces);
                wlHitsWithFaces.Add(wlHitWithFaces);
            });

            IEnumerable res = LocalQuery<WlHitWithPerson>(linq, linqSelectExpression, wlHitsWithFaces);
            return res;
        }
    }
}