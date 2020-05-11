namespace SmartFace.Cli.Common
{
    public static class Constants
    {
        public const int EXIT_CODE_OK = 0;
        public const int EXIT_CODE_GENERAL_ERROR = 1;

        public const string ARGUMENT_HOST = "--host";
        public const string ENVIRONMENT_HOST = "sfcli_host";

        public const string HELP_SPECIFY_SUB_CMD = "You must specify a sub-command. Use -h to help.";
        
        public const string JPEG = "jpeg";
        public const string JPG = "jpg";
        public const string PNG = "png";
        public const string JPEG_MIME_TYPE = "image/" + JPEG;
        public const string PNG_MIME_TYPE = "image/" + PNG;
    }
}