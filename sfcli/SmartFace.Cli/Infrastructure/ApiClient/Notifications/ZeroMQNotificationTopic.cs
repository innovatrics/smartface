namespace SmartFace.Cli.Infrastructure.ApiClient.Notifications
{
    public class ZeroMqNotificationTopic
    {
        public const string FACE_CREATED = "faces.insert";
        public const string FACE_EXTRACTED = "faces.extracted";
        public const string INPUT_FILE_STATE_UPDATE = "inputFiles.update";
        public const string PERSON_COMPLETED = "persons.completed";
        public const string GROUPING_PROGRESS_INFO = "grouping_progress.info";
        public const string MATCH_RESULT = "matchResults";
        public const string MATCH_RESULT_INSERTED = "matchResults.insert";
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
                MATCH_RESULT,
                MATCH_RESULT_INSERTED,
                HEARTBEAT
            };
        }
    }    
}
