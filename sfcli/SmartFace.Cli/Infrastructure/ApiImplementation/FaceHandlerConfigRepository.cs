using System.Linq;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.Cli.Infrastructure.ApiClient.Models;
using SmartFace.ODataClient.Default;

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
                (DetectionAlgorithm.Fast, "fast"),
                (DetectionAlgorithm.Accurate, "accurate"),
                (DetectionAlgorithm.Balanced, "balanced")
            };

        private Container Container { get; }


        public FaceHandlerConfigRepository(Container container)
        {
            Container = container;
        }

        public FaceHandlerConfigModel Get()
        {
            var config = Container.Configs.GetConfigValuesEx(Constants.CONFIG_NAME_FACE_HANDLER_CONFIG, CONTEXT);
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
            Container.Configs.SetConfigValuesEx(UpdateConfigValuesDescriptor.Create(
                new UpdateConfigValueData
                {
                    Name = Constants.CONFIG_NAME_FACE_HANDLER_CONFIG,
                    Context = CONTEXT,
                    Property = PROP_DETECTION_MIN_EYE_DISTANCE,
                    Value = configModel.DetectionMinEyeDistance
                }, new UpdateConfigValueData
                {
                    Name = Constants.CONFIG_NAME_FACE_HANDLER_CONFIG,
                    Context = CONTEXT,
                    Property = PROP_DETECTION_MAX_EYE_DISTANCE,
                    Value = configModel.DetectionMaxEyeDistance
                }, new UpdateConfigValueData
                {
                    Name = Constants.CONFIG_NAME_FACE_HANDLER_CONFIG,
                    Context = CONTEXT,
                    Property = PROP_FACE_CONFIDENCE_THRESHOLD,
                    Value = configModel.FaceConfidenceThreshold
                }, new UpdateConfigValueData
                {
                    Name = Constants.CONFIG_NAME_FACE_HANDLER_CONFIG,
                    Context = CONTEXT,
                    Property = PROP_DETECTION_ALGORITHM,
                    Value = GetDetectionAlgorithmName(configModel.DetectionAlgorithm)
                }));
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
