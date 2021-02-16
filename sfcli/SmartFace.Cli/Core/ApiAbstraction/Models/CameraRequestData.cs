using System.Collections.Generic;

namespace SmartFace.Cli.Core.ApiAbstraction.Models
{
    public class CameraRequestData
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public bool? Enabled { get; set; }
        public int? TrackMinFaceSize { get; set; }
        public int? TrackMaxFaceSize { get; set; }
        public int? MPEG1PreviewPort { get; set; }
        public int? RedetectionTime { get; set; }
        public string FaceDetectorResourceId { get; set; }
        public string TemplateGeneratorResourceId { get; set; }
        public string PedestrianDetectorResourceId { get; set; }
        public float? MinPedestrianSize { get; set; }
        public float? MaxPedestrianSize { get; set; }
        public List<string> SpoofDetectorResourceIds { get; set; }
    }
}