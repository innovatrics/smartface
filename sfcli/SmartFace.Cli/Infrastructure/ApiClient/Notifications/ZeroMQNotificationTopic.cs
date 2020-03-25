namespace SmartFace.Cli.Infrastructure.ApiClient.Notifications
{
    public class ZeroMqNotificationTopic
    {
        public const string FACE_CREATED = "faces.insert";
        public const string FACE_EXTRACTED = "faces.extracted";
        public const string INPUT_FILE_STATE_UPDATE = "inputFiles.update";
        public const string PERSON_COMPLETED = "persons.completed";
        public const string GROUPING_PROGRESS_INFO = "grouping_progress.info";
        public const string MATCH_RESULT_MATCH = "matchResults.match";
        public const string MATCH_RESULT_NO_MATCH = "matchResults.nomatch";
        public const string MATCH_RESULT_MATCH_INSERTED = "matchResults.match.insert";
        public const string HEARTBEAT = "heartbeat";
    }    
}
