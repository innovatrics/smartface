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
        public string ObjectDetectorResourceId { get; set; }
        public int? MinObjectSize { get; set; }
        public int? MaxObjectSize { get; set; }
    }
}