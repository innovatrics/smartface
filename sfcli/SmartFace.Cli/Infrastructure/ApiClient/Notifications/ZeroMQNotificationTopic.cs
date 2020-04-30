namespace SmartFace.Cli.Infrastructure.ApiClient.Notifications
{
    public class ZeroMqNotificationTopic
    {
        public const string FACE_CREATED = "faces.insert";
        public const string FACE_EXTRACTED = "faces.extracted";
        public const string INPUT_FILE_STATE_UPDATE = "inputFiles.update";
        public const string PERSON_COMPLETED = "persons.completed";
        public const string GROUPING_PROGRESS_INFO = "grouping_progress.info";
        public const string WATCHLIST_ITEM_HIT = "wlHits.match";
        public const string WATCHLIST_ITEM_INSERTED = "wlHits.insert";
        public const string LIVENESS_RESULT = "liveness.result";
        public const string HEARTBEAT = "heartbeat";

        public static string[] GetAll()
        {
            return new[]
            {
                FACE_CREATED,
                FACE_EXTRACTED,
                INPUT_FILE_STATE_UPDATE,
                PERSON_COMPLETED,
                GROUPING_PROGRESS_INFO,
                WATCHLIST_ITEM_HIT,
                WATCHLIST_ITEM_INSERTED,
                LIVENESS_RESULT,
                HEARTBEAT
            };
        }
    }    
}
