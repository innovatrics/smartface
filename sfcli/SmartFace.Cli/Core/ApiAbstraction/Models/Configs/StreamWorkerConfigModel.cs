namespace SmartFace.Cli.Core.ApiAbstraction.Models.Configs
{
    public class StreamWorkerConfigModel : IStreamWorkerConfig
    {
        public long? TrackMinEyeDistance { get; set; }
        
        public long? TrackMaxEyeDistance { get; set; }
        
        public long? MjpegPreviewPort { get; set; }
        
        public long? FaceDiscoveryFrequence { get; set; }
    }
}