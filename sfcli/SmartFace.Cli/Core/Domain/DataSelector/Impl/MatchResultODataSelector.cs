using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using SmartFace.Cli.Common.Utils;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector.Impl
{
    public class MatchResultODataSelector : ODataSelector<MatchResult>, IQueryDataSelector<MatchResult>
    {
        private const string ALLOWED_EXPAND_TRACKLET = "Tracklet";
        private const string ALLOWED_EXPAND_FACES = "Tracklet($expand=Faces)";

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

            var matchResultWithTracklets = new ConcurrentBag<MatchResultWithTracklet>();
            Parallel.ForEach(entities, (matchResult) =>
            {
                DataServiceQuery<Tracklet> baseQuery;
                if (ALLOWED_EXPAND_TRACKLET.Equals(expandProperty, StringComparison.InvariantCultureIgnoreCase))
                {
                    baseQuery = Api.Tracklets;
                }
                else if (ALLOWED_EXPAND_FACES.Equals(expandProperty, StringComparison.InvariantCultureIgnoreCase))
                {
                    baseQuery = Api.Tracklets.Expand(p => p.Faces);
                }
                else
                {
                    throw new NotSupportedException();
                }

                var query = (DataServiceQuery<Tracklet>)baseQuery.Where(p => p.Id == matchResult.TrackletId);
                var tracklet = query.ExecuteAsync().Result.SingleOrDefault();
                var matchResultWithTracklet = new MatchResultWithTracklet(tracklet);
                matchResult.CopyProperties(matchResultWithTracklet);
                matchResultWithTracklets.Add(matchResultWithTracklet);
            });

            var res = LocalQuery<MatchResultWithTracklet>(linq, linqSelectExpression, matchResultWithTracklets);
            return res;
        }
    }
}