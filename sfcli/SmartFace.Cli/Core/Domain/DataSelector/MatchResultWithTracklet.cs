using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector
{
    public class MatchResultWithTracklet : MatchResult
    {
        public Tracklet Tracklet { get; }

        public MatchResultWithTracklet(Tracklet tracklet)
        {
            Tracklet = tracklet;
        }
    }
}
