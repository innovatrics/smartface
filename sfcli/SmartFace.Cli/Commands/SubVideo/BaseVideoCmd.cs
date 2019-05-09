using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.Domain.StreamProcessor;

namespace SmartFace.Cli.Commands.SubVideo
{
    public class BaseVideoCmd
    {
        
        [Option("-v|--videoSource", "Url to video E.g. rtsp://server.example.org:8080/test.sdp", CommandOptionType.SingleValue)]
        public (bool HasValue, string Value) VideoSource { get; }
        
        [Option("-e|--enabled", "Whether the stream is processed or not", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) Enabled { get; }
        
        [Option("-i|--minEyeDistance", "Minimum count of pixels between eyes", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) TrackMinEyeDistance { get; }
        
        [Option("-x|--maxEyeDistance", "Maximum count of pixels between eyes", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) TrackMaxEyeDistance { get; }
        
        [Option("-d|--faceDiscovery", "Time between face re-detections in milliseconds", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) FaceDiscoveryFrequence { get; }
        
        [Option("-p|--mjpegPreviewPort", "Port to processed stream MJPEG preview", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) MjpegPreviewPort { get; }

        protected void SetBaseParameters(VideoProcessor videoProcessor)
        {
            if (VideoSource.HasValue)
            {
                videoProcessor.VideoSource = VideoSource.Value;
            }
            
            if (Enabled.HasValue)
            {
                videoProcessor.Enabled = Enabled.Value;
            }

            if (TrackMinEyeDistance.HasValue)
            {
                videoProcessor.TrackMinEyeDistance = TrackMinEyeDistance.Value;
            }

            if (TrackMaxEyeDistance.HasValue)
            {
                videoProcessor.TrackMaxEyeDistance = TrackMaxEyeDistance.Value;
            }

            if (FaceDiscoveryFrequence.HasValue)
            {
                videoProcessor.FaceDiscoveryFrequence = FaceDiscoveryFrequence.Value;
            }

            if (MjpegPreviewPort.HasValue)
            {
                videoProcessor.MjpegPreviewPort = MjpegPreviewPort.Value;
            }
        }
    }
}