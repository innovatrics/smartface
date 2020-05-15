namespace SmartFace.Cli.Core.Domain
{
    public static class ResourceIds
    {
        public class TemplateGeneratorResourceId
        {
            public const string BALANCED_CPU = "balanced_cpu";
            public const string BALANCED_GPU = "balanced_gpu";

            public static readonly string Default = BALANCED_CPU;
        }

        public class FaceDetectorResourceId
        {
            public const string FAST = "fast";
            public const string BALANCED_CPU = "balanced_cpu";
            public const string BALANCED_GPU = "balanced_gpu";
            public const string ACCURATE_CPU = "accurate_cpu";
            public const string ACCURATE_GPU = "accurate_gpu";

            public static readonly string Default = FAST;
        }
    }
}