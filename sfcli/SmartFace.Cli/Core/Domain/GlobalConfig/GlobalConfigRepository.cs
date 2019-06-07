using AutoMapper;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.Core.Domain.GlobalConfig
{
    public class GlobalConfigRepository : IGlobalConfigRepository
    {
        private IApiProvider ApiProvider { get; }

        private IMapper Mapper { get; }

        public GlobalConfigRepository(IApiProvider apiProvider, IMapper mapper)
        {
            ApiProvider = apiProvider;
            Mapper = mapper;
        }

        public GlobalConfig Get()
        {
            var fhConfig = ApiProvider.FaceHandlerConfigs.Get();
            var ifaceConfig = ApiProvider.IFaceConfigs.Get(IFaceConfigKey.Camera);
            var globalConfig = Mapper.Map<GlobalConfig>(fhConfig);
            globalConfig = Mapper.Map(ifaceConfig, globalConfig);
            return globalConfig;
        }

        public void Set(GlobalConfig config)
        {
            var fhConfig = Mapper.Map<FaceHandlerConfigModel>(config);
            var ifaceConfig = Mapper.Map<IFaceConfigModel>(config);
            ApiProvider.FaceHandlerConfigs.Set(fhConfig);
            ApiProvider.IFaceConfigs.Set(IFaceConfigKey.Camera, ifaceConfig);
            ApiProvider.IFaceConfigs.Set(IFaceConfigKey.Grouping, ifaceConfig);
            ApiProvider.IFaceConfigs.Set(IFaceConfigKey.Detect, ifaceConfig);
            ApiProvider.IFaceConfigs.Set(IFaceConfigKey.Extract, ifaceConfig);
        }
    }
}