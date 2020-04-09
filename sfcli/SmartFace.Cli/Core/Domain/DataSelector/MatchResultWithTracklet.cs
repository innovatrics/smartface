using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector
{
    public class MatchResultWithTracklet : MatchResult
    {
        public Person Tracklet { get; }

        public MatchResultWithTracklet(Person tracklet)
        {
            Tracklet = tracklet;
        }
    }
}
