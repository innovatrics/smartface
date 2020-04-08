using NSubstitute;
using SmartFace.Cli.Common.DI;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Core.Domain.GlobalConfig;
using Tests;

namespace SmartFace.CliTests.SfCliTests.Domain.GlobalConfig
{

    public class GlobalConfigContext
    {
        public SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig Config { get; set; }

        public IGlobalConfigRepository Repository { get; }

        public IApiProvider ApiProvider { get; } = Tests.Utils.SubstituteApiProvider();

        public GlobalConfigContext()
        {
            Repository = new GlobalConfigRepository(ApiProvider, Installer.ConfigureAutoMapper());
        }

    }

    public class SetGlobalConfig_IFaceConfigCamera : Scenario<GlobalConfigContext>
    {
        private given _gpu_enabled = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { GpuEnabled = true };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _iface_config_camera_gpu_is_set_to_true = _ =>
            _.ApiProvider.IFaceConfigs.Received().Set(Arg.Is(IFaceConfigKey.Camera),
                Arg.Is<IFaceConfigModel>((model) => model.GpuEnabled == _.Config.GpuEnabled));
    }

    public class SetGlobalConfig_IFaceConfigDetect : Scenario<GlobalConfigContext>
    {
        private given _gpu_enabled = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { GpuEnabled = true };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _iface_config_detect_gpu_is_set_to_true = _ =>
            _.ApiProvider.IFaceConfigs.Received().Set(Arg.Is(IFaceConfigKey.Detect),
                Arg.Is<IFaceConfigModel>((model) => model.GpuEnabled == _.Config.GpuEnabled));
    }

    public class SetGlobalConfig_IFaceConfigExtract : Scenario<GlobalConfigContext>
    {
        private given _gpu_enabled = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { GpuEnabled = true };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _iface_config_extract_gpu_is_set_to_true = _ =>
            _.ApiProvider.IFaceConfigs.Received().Set(Arg.Is(IFaceConfigKey.Extract),
                Arg.Is<IFaceConfigModel>((model) => model.GpuEnabled == _.Config.GpuEnabled));
    }

    public class SetGlobalConfig_IFaceConfigGrouping : Scenario<GlobalConfigContext>
    {
        private given _gpu_enabled = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { GpuEnabled = true };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _iface_config_grouping_gpu_is_set_to_true = _ =>
            _.ApiProvider.IFaceConfigs.Received().Set(Arg.Is(IFaceConfigKey.Grouping),
                Arg.Is<IFaceConfigModel>((model) => model.GpuEnabled == _.Config.GpuEnabled));
    }

    public class SetGlobalConfig_FaceHandler_MinEye : Scenario<GlobalConfigContext>
    {
        private given _min_eye = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { DetectionMinEyeDistance = 43 };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _fh_config_min_eye_distance_is_set = _ =>
            _.ApiProvider.FaceHandlerConfigs.Received().Set(Arg.Is<FaceHandlerConfigModel>((model) => model.DetectionMinEyeDistance == _.Config.DetectionMinEyeDistance));
    }

    public class SetGlobalConfig_FaceHandler_MaxEye : Scenario<GlobalConfigContext>
    {
        private given _max_eye = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { DetectionMaxEyeDistance = 2345 };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _fh_config_max_eye_distance_is_set = _ =>
            _.ApiProvider.FaceHandlerConfigs.Received().Set(Arg.Is<FaceHandlerConfigModel>((model) => model.DetectionMaxEyeDistance == _.Config.DetectionMaxEyeDistance));
    }

    public class SetGlobalConfig_FaceHandler_Confidence : Scenario<GlobalConfigContext>
    {
        private given _confidence = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { FaceConfidenceThreshold = 543 };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _fh_config_confidence_is_set = _ =>
            _.ApiProvider.FaceHandlerConfigs.Received().Set(Arg.Is<FaceHandlerConfigModel>((model) => model.FaceConfidenceThreshold == _.Config.FaceConfidenceThreshold));
    }

    public class SetGlobalConfig_FaceHandler_DetectionAlgorithm : Scenario<GlobalConfigContext>
    {
        private given _algorithm_gpu = _ => _.Config = new SmartFace.Cli.Core.Domain.GlobalConfig.GlobalConfig { DetectionAlgorithm = DetectionAlgorithm.Balanced };

        private when _set_config = _ => _.Repository.Set(_.Config);

        private then _fh_config_detection_algorithm_is_set = _ =>
            _.ApiProvider.FaceHandlerConfigs.Received().Set(Arg.Is<FaceHandlerConfigModel>((model) => model.DetectionAlgorithm == _.Config.DetectionAlgorithm));
    }
}