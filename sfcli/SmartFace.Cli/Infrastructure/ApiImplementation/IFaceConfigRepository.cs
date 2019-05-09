using System.Linq;
using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.Cli.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Common;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.Extensions;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class IFaceConfigRepository : IIFaceConfigRepository
    {
        private const string PROP_GPU_ENABLED = "GpuEnabled";

        private static readonly (IFaceConfigKey DomainKey, string ApiKey)[] KeysMapping =
            {
                (IFaceConfigKey.Camera, "SmartFace_Camera"),
                (IFaceConfigKey.Detect, "SmartFace_Detect"),
                (IFaceConfigKey.Extract, "SmartFace_Extract"),
                (IFaceConfigKey.Grouping, "SmartFace_Grouping"),
            };

        private Container Container { get; }

        public IFaceConfigRepository(Container container)
        {
            Container = container;
        }

        public IFaceConfigModel Get(IFaceConfigKey key)
        {
            var apiKey = GetApiKey(key);
            var config = Container.Configs.GetConfigValuesEx(Constants.ConfigName_IFaceConfig, apiKey);
            return new IFaceConfigModel
            {
                GpuEnabled = (bool)config[PROP_GPU_ENABLED]
            };
        }

        public void Set(IFaceConfigKey key, IFaceConfigModel configModel)
        {
            var apiKey = GetApiKey(key);
            Container.Configs.SetConfigValueEx(Constants.ConfigName_IFaceConfig, apiKey, PROP_GPU_ENABLED, configModel.GpuEnabled);
        }

        private static string GetApiKey(IFaceConfigKey domainKey)
        {
            return KeysMapping.Single(m => m.DomainKey == domainKey).ApiKey;
        }
    }
}
