using SmartFace.Cli.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.Core.Domain.GlobalConfig
{
    public class GlobalConfig
    {
        public bool GpuEnabled { get; set; }

        public long FaceConfidenceThreshold { get; set; }

        public long DetectionMinEyeDistance { get; set; }

        public long DetectionMaxEyeDistance { get; set; }

        public DetectionAlgorithm DetectionAlgorithm { get; set; }
    }
}