using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Commands.SubCamera
{
    [Command(Name = "get", Description = "Read properties of camera")]
    public class GetCameraCmd
    {
        [Option("-i|--id", "Id of camera to get. If empty, all cameras will be fetched.", CommandOptionType.SingleValue)]
        public (bool HasValue, Guid Value) CameraId { get; }
        
        private ICamerasRepository Repository { get; }
        
        public GetCameraCmd(ICamerasRepository repository)
        {
            Repository = repository;
        }
        
        protected virtual async Task OnExecuteAsync(IConsole console)
        {
            object result;

            if (CameraId.HasValue)
            {
                result = await Repository.GetCameraAsync(CameraId.Value);
            }
            else
            {
                result = await Repository.GetCamerasAsync();
            }

            var resultOutput = JsonConvert.SerializeObject(result, Formatting.Indented);
            console.WriteLine(resultOutput);
        }
    }
}