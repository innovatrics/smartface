using SmartFace.Cli.Infrastructure.ApiClient.Notifications;

namespace SmartFace.Cli.Common
{
    public static class Constants
    {
        public const int EXIT_CODE_OK = 0;

        public const int EXIT_CODE_GENERAL_ERROR = 1;

        public const string ArgumentUrl = "--url";

        public const string EnvironmentUrl = "sfcli_url";


        public const string HELP_SPECIFY_SUBCMD = "You must specify a sub-command. Use -h to help.";


        public const string ConfigName_StreamWorkerConfig = "StreamWorkerConfig";

        public const string ConfigName_VideoPublishWorkerConfig = "VideoPublishWorkerConfig";

        public const string ConfigName_FaceHandlerConfig = "FaceHandlerConfig";

        public const string ConfigName_IFaceConfig = "IFaceConfig";






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
            ZeroMqNotificationTopic.MATCH_RESULT,
            ZeroMqNotificationTopic.MATCH_RESULT_INSERTED
        };
    }
}