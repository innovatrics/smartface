namespace SmartFace.Cli.Core.ApiAbstraction.Models.Configs
{
    public enum DetectionAlgorithm
    {
        CpuFast,
        CpuAccurate,
        Gpu
    }

    public class FaceHandlerConfigModel
    {

        public long FaceConfidenceThreshold { get; set; }

        public long DetectionMinEyeDistance { get; set; }

        public long DetectionMaxEyeDistance { get; set; }

        public DetectionAlgorithm DetectionAlgorithm { get; set; }
    }
}
