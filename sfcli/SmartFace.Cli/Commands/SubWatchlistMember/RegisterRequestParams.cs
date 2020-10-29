using System;

namespace SmartFace.Cli.Commands.SubWatchlistMember
{
    public class RegisterRequestParams
    {
        public RegisterRequestParams(int minFaceSize, int maxFaceSize, int faceConfidenceThreshold, string faceDetectionResourceId, string templateGeneratorResourceId, string[] watchlistIds)
        {
            MinFaceSize = minFaceSize;
            MaxFaceSize = maxFaceSize;
            FaceConfidenceThreshold = faceConfidenceThreshold;
            FaceDetectionResourceId = faceDetectionResourceId;
            TemplateGeneratorResourceId = templateGeneratorResourceId;
            WatchlistIds = watchlistIds ?? throw new ArgumentNullException(nameof(watchlistIds));

            if (WatchlistIds.Length == 0)
            {
                throw new ArgumentException("WatchlistIDs cannot be empty array.");
            }
        }

        public int MinFaceSize { get; }
        public int MaxFaceSize { get; }
        public int FaceConfidenceThreshold { get; }
        public string FaceDetectionResourceId { get; }
        public string TemplateGeneratorResourceId { get; }
        public string[] WatchlistIds { get; }
    }
}
