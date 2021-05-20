namespace SmartFace.Cli.Common
{
    public static class Constants
    {
        public const int EXIT_CODE_OK = 0;
        public const int EXIT_CODE_GENERAL_ERROR = 1;

        public const string ARGUMENT_URL_API = "--api-url";
        public const string ARGUMENT_URL_ODATA = "--odata-url";
        
        public const string ENVIRONMENT_URL_API = "sfcli_url_api";
        public const string ENVIRONMENT_URL_ODATA = "sfcli_url_odata";
        
        public const string DEFAULT_URL_API = "http://localhost:8098";
        public const string DEFAULT_URL_ODATA = "http://localhost:8099";

        public const string HELP_SPECIFY_SUB_CMD = "You must specify a sub-command. Use -h to help.";
        
        public const string JPEG = "jpeg";
        public const string JPG = "jpg";
        public const string PNG = "png";
    }
}