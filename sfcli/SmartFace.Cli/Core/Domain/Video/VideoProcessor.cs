using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmartFace.Cli.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.Domain.StreamProcessor
{
    public class VideoProcessor : IVideoPublishWorkerConfig, IStreamWorkerConfig
    {
        public long StreamId { get; set; }
        
        public long? ScopeId { get; set; }
        
        public string VideoSource { get; set; }
        
        public bool? Enabled { get; set; }
        
        public long? TrackMinEyeDistance { get; set; }
        
        public long? TrackMaxEyeDistance { get; set; }
        
        public long? MjpegPreviewPort { get; set; }
        
        public long? FaceDiscoveryFrequence { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StreamType? Type { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public StreamState? State { get; set; }
    }
}