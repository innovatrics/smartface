using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Commands.SubCamera
{
    public abstract class BaseCameraModifyingCmd
    {   
        [Option("-e|--enabled", "Whether the stream is processed or not", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) Enabled { get; }
        
        [Option("-m|--minFaceSize", "Minimum count of pixels between eyes", CommandOptionType.SingleValue)]
        public (bool HasValue, int Value) TrackMinFaceSize { get; }
        
        [Option("-x|--maxFaceSize", "Maximum count of pixels between eyes", CommandOptionType.SingleValue)]
        public (bool HasValue, int Value) TrackMaxFaceSize { get; }
        
        [Option("-r|--redetectionTime", "Time between face re-detections in milliseconds", CommandOptionType.SingleValue)]
        public (bool HasValue, int Value) RedetectionTime { get; }
        
        [Option("-p|--mpeg1PreviewPort", "Port to processed stream MPEG1 preview", CommandOptionType.SingleValue)]
        public (bool HasValue, int Value) MPEG1PreviewPort { get; }

        [Option("-tg|--templateGeneratorResourceId", "Template generator resource id for the camera", CommandOptionType.SingleValue)]
        public (bool HasValue, string Value) TemplateGeneratorResourceId { get; }

        [Option("-fd|--faceDetectorResourceId", "Face detector resource id for the camera", CommandOptionType.SingleValue)]
        public (bool HasValue, string Value) FaceDetectorResourceId { get; }

        [Option("-pd|--pedestrianDetectorResourceId", "Pedestrian detector resource id for the camera", CommandOptionType.SingleValue)]
        public (bool HasValue, string Value) PedestrianDetectorResourceId { get; }

        [Option("-mp|--minPedestrianSize", "Minimum size of detected pedestrian in pixels (if >= 1) or relative to the longer edge of the processed video (if > 0 && < 1)", CommandOptionType.SingleValue)]
        public (bool HasValue, float Value) MinPedestrianSize { get; }
        
        [Option("-xp|--maxPedestrianSize", "Maximum size of detected pedestrian in pixels (if >= 1) or relative to the longer edge of the processed video (if > 0 && < 1)", CommandOptionType.SingleValue)]
        public (bool HasValue, float Value) MaxPedestrianSize { get; }

        [Option("-sd|--spoofDetectorResourceId", "Spoof detector resource id for the camera", CommandOptionType.MultipleValue)]
        public List<string> SpoofDetectorResourceIds { get; }

        public abstract (bool HasValue, string Value) Name { get; }
        public abstract (bool HasValue, string Value) Source { get; }

        protected void SetBaseParameters(CameraRequestData cameraRequestData)
        {
            if (Source.HasValue)
            {
                cameraRequestData.Source = Source.Value;
            }
            
            if (Enabled.HasValue)
            {
                cameraRequestData.Enabled = Enabled.Value;
            }

            if (TrackMinFaceSize.HasValue)
            {
                cameraRequestData.TrackMinFaceSize = TrackMinFaceSize.Value;
            }

            if (TrackMaxFaceSize.HasValue)
            {
                cameraRequestData.TrackMaxFaceSize = TrackMaxFaceSize.Value;
            }

            if (RedetectionTime.HasValue)
            {
                cameraRequestData.RedetectionTime = RedetectionTime.Value;
            }

            if (MPEG1PreviewPort.HasValue)
            {
                cameraRequestData.MPEG1PreviewPort = MPEG1PreviewPort.Value;
            }

            if (TemplateGeneratorResourceId.HasValue)
            {
                cameraRequestData.TemplateGeneratorResourceId = TemplateGeneratorResourceId.Value;
            }

            if (FaceDetectorResourceId.HasValue)
            {
                cameraRequestData.FaceDetectorResourceId = FaceDetectorResourceId.Value;
            }

            if (PedestrianDetectorResourceId.HasValue)
            {
                cameraRequestData.PedestrianDetectorResourceId = PedestrianDetectorResourceId.Value;
            }

            if (MinPedestrianSize.HasValue)
            {
                cameraRequestData.MinPedestrianSize = MinPedestrianSize.Value;
            }

            if (MaxPedestrianSize.HasValue)
            {
                cameraRequestData.MaxPedestrianSize = MaxPedestrianSize.Value;
            }

            if (Name.HasValue)
            {
                cameraRequestData.Name = Name.Value;
            }

            cameraRequestData.SpoofDetectorResourceIds = SpoofDetectorResourceIds;
        }
    }
}