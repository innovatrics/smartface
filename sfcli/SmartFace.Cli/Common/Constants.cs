using SmartFace.Cli.Infrastructure.ApiClient.Notifications;

namespace SmartFace.Cli.Common
{
    public static class Constants
    {
        public const int EXIT_CODE_OK = 0;
        public const int EXIT_CODE_GENERAL_ERROR = 1;

        public const string ARGUMENT_URL = "--url";
        public const string ENVIRONMENT_URL = "sfcli_url";

        public const string HELP_SPECIFY_SUB_CMD = "You must specify a sub-command. Use -h to help.";

        public const string CONFIG_NAME_STREAM_WORKER_CONFIG = "StreamWorkerConfig";
        public const string CONFIG_NAME_VIDEO_PUBLISH_WORKER_CONFIG = "VideoPublishWorkerConfig";
        public const string CONFIG_NAME_FACE_HANDLER_CONFIG = "FaceHandlerConfig";
        public const string CONFIG_NAME_I_FACE_CONFIG = "IFaceConfig";
        
        public const string JPEG = "jpeg";
        public const string JPG = "jpg";
        public const string PNG = "png";
        public const string JPEG_MIME_TYPE = "image/" + JPEG;
        public const string PNG_MIME_TYPE = "image/" + PNG;

        public static readonly string[] ZeroMqTopics = new[]
        {
            ZeroMqNotificationTopic.FACE_CREATED,
            ZeroMqNotificationTopic.GROUPING_PROGRESS_INFO,
            ZeroMqNotificationTopic.INPUT_FILE_STATE_UPDATE,
            ZeroMqNotificationTopic.PERSON_COMPLETED,
            ZeroMqNotificationTopic.MATCH_RESULT_MATCH,
            ZeroMqNotificationTopic.MATCH_RESULT_NO_MATCH,
            ZeroMqNotificationTopic.MATCH_RESULT_MATCH_INSERTED
        };
    }
}