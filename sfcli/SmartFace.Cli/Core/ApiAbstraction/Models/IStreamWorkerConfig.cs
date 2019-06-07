namespace SmartFace.Cli.Core.ApiAbstraction.Models
{
    public interface IStreamWorkerConfig
    {
        long? TrackMinEyeDistance { get; set; }
        
        long? TrackMaxEyeDistance { get; set; }
        
        long? MjpegPreviewPort { get; set; }
        
        long? FaceDiscoveryFrequence { get; set; }
    }
}