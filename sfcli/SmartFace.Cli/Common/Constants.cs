namespace SmartFace.Cli.Common
{
    public static class Constants
    {
        public const int EXIT_CODE_OK = 0;
        public const int EXIT_CODE_GENERAL_ERROR = 1;

        public const string ARGUMENT_HOST = "--host";
        public const string ENVIRONMENT_HOST = "sfcli_host";

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
    }
}