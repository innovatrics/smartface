namespace SmartFace.Cli.Core.Domain
{
    public static class ResourceIds
    {
        public static class TemplateGenerator
        {
            public const string WILD_CPU = "WildCpu";
            public const string WILD_GPU = "WildGpu";
        }

        public static class FaceDetector
        {
            public const string ACCURATE_CPU = "AccurateCpu";
            public const string ACCURATE_GPU = "AccurateGpu";
            public const string BALANCED_CPU = "BalancedCpu";
            public const string BALANCED_GPU = "BalancedGpu";
            public const string FAST = "Fast";
        }
    }
}