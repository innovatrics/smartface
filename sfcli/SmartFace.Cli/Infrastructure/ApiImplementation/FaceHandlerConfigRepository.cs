using System.Linq;
using SmartFace.Cli.ApiAbstraction;
using SmartFace.Cli.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Common;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.Extensions;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class FaceHandlerConfigRepository : IFaceHandlerConfigRepository
    {
        private const string PROP_DETECTION_MIN_EYE_DISTANCE = "MinEyeDistance";
        private const string PROP_DETECTION_MAX_EYE_DISTANCE = "MaxEyeDistance";
        private const string PROP_DETECTION_ALGORITHM = "FaceDetSpeedAccuracyMode";
        private const string PROP_FACE_CONFIDENCE_THRESHOLD = "FaceDetectionConfidenceThreshold";
        private const string CONTEXT = ".";

        private static readonly (DetectionAlgorithm DomainType, string ApiName)[] DetectionAlgorithmMapping =
            {
                (DetectionAlgorithm.CpuFast, "fast"),
                (DetectionAlgorithm.CpuAccurate, "accurate"),
                (DetectionAlgorithm.Gpu, "accurate_server")
            };

        private Container Container { get; }


        public FaceHandlerConfigRepository(Container container)
        {
            Container = container;
        }

        public FaceHandlerConfigModel Get()
        {
            var config = Container.Configs.GetConfigValuesEx(Constants.ConfigName_FaceHandlerConfig, CONTEXT);
            return new FaceHandlerConfigModel
            {
                DetectionMinEyeDistance = (long)config[PROP_DETECTION_MIN_EYE_DISTANCE],
                DetectionMaxEyeDistance = (long)config[PROP_DETECTION_MAX_EYE_DISTANCE],
                DetectionAlgorithm = GetDetectionAlgorithmType((string)config[PROP_DETECTION_ALGORITHM]),
                FaceConfidenceThreshold = (long)config[PROP_FACE_CONFIDENCE_THRESHOLD]
            };
        }

        public void Set(FaceHandlerConfigModel configModel)
        {
            Container.Configs.SetConfigValueEx(Constants.ConfigName_FaceHandlerConfig, CONTEXT, PROP_DETECTION_MIN_EYE_DISTANCE, configModel.DetectionMinEyeDistance);
            Container.Configs.SetConfigValueEx(Constants.ConfigName_FaceHandlerConfig, CONTEXT, PROP_DETECTION_MAX_EYE_DISTANCE, configModel.DetectionMaxEyeDistance);
            Container.Configs.SetConfigValueEx(Constants.ConfigName_FaceHandlerConfig, CONTEXT, PROP_FACE_CONFIDENCE_THRESHOLD, configModel.FaceConfidenceThreshold);
            Container.Configs.SetConfigValueEx(Constants.ConfigName_FaceHandlerConfig, CONTEXT, PROP_DETECTION_ALGORITHM, GetDetectionAlgorithmName(configModel.DetectionAlgorithm));
        }

        private static DetectionAlgorithm GetDetectionAlgorithmType(string name)
        {
            return DetectionAlgorithmMapping.Single(m => m.ApiName == name).DomainType;
        }
        private static string GetDetectionAlgorithmName(DetectionAlgorithm type)
        {
            return DetectionAlgorithmMapping.Single(m => m.DomainType == type).ApiName;
        }
    }
}
