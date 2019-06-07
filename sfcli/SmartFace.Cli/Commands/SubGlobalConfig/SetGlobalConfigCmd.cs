using System;
using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Core.Domain.GlobalConfig;

namespace SmartFace.Cli.Commands.SubGlobalConfig
{
    [Command(Name = "set", Description = "Edit properties of GlobalConfig")]
    public class SetGlobalConfigCmd
    {
        private const string CpuFast = "CpuFast";
        private const string CpuAccurate = "CpuAccurate";
        private const string Gpu = "Gpu";
        private IGlobalConfigRepository Repository { get; }

        [Option("-i|--minEyeDistance", "Minimum count of pixels between eyes (detection on photo)", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) MinEyeDistance { get; }

        [Option("-x|--maxEyeDistance", "Maximum count of pixels between eyes (detection on photo)", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) MaxEyeDistance { get; }

        [Option("-c|--faceConfidence", "Face confidence threshold. For cpu detection algorithm around [450]. For gpu algorithm should be set around [7000]", CommandOptionType.SingleValue)]
        public (bool HasValue, long Value) FaceConfidence { get; }

        [Option("-a|--detectionAlgorithm", "Specify type of algorithm used for face detection. " + Gpu + " algorithm is slow if you don't have GPU enabled. ["
            + CpuFast + ", "
            + CpuAccurate + ", "
            + Gpu + "]", CommandOptionType.SingleValue)]
        [AllowedValues(CpuFast, CpuAccurate, Gpu, IgnoreCase = false)]
        public (bool HasValue, string Value) DetectAlgorithm { get; }

        [Option("-g|--gpu", "Enable/Disable GPU support. If CPU detection algorithm is used then is GPU card used only for extractions.", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) GpuEnabled { get; }

        public SetGlobalConfigCmd(IGlobalConfigRepository repository)
        {
            Repository = repository;
        }

        protected virtual void OnExecute(IConsole console)
        {
            var config = Repository.Get();

            if (MinEyeDistance.HasValue)
            {
                config.DetectionMinEyeDistance = MinEyeDistance.Value;
            }

            if (MaxEyeDistance.HasValue)
            {
                config.DetectionMaxEyeDistance = MaxEyeDistance.Value;
            }

            if (FaceConfidence.HasValue)
            {
                config.FaceConfidenceThreshold = FaceConfidence.Value;
            }

            if (DetectAlgorithm.HasValue)
            {
                var detectionAlgorithm = Enum.Parse<DetectionAlgorithm>(DetectAlgorithm.Value);
                config.DetectionAlgorithm = detectionAlgorithm;
            }

            if (GpuEnabled.HasValue)
            {
                config.GpuEnabled = GpuEnabled.Value;
            }

            Repository.Set(config);

            var output = JsonConvert.SerializeObject(config, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                Formatting = Formatting.Indented
            });
            console.WriteLine(output);
        }
    }
}
