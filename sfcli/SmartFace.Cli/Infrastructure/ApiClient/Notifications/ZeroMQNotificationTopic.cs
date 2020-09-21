namespace SmartFace.Cli.Infrastructure.ApiClient.Notifications
{
    public class ZeroMqNotificationTopic
    {
        public const string FACE_CREATED = "faces.insert";
        public const string FACE_EXTRACTED = "faces.extracted";
        public const string VIDEO_RECORD_STATE_UPDATE = "videoRecords.update";
        public const string TRACKLET_COMPLETED = "tracklets.completed";
        public const string GROUPING_PROGRESS_INFO = "grouping_progress.info";
        public const string MATCH_RESULT_MATCH = "matchResults.match";
        public const string MATCH_RESULT_NO_MATCH = "matchResults.nomatch";
        public const string MATCH_RESULT_MATCH_INSERTED = "matchResults.match.insert";
        public const string LIVENESS_RESULT = "liveness.result";
        public const string HEARTBEAT = "heartbeat";
        public const string PEDESTRIANS_INSERTED = "pedestrians.insert";
    }    
}
