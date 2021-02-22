using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Commands.SubCamera
{
    [Command(Name = "delete", Description = "Delete a camera")]
    public class DeleteCameraCmd
    {
        [Option("-i|--id", "Id of camera to delete.", CommandOptionType.SingleValue)]
        public (bool HasValue, Guid Value) CameraId { get; }

        private ICamerasRepository Repository { get; }

        public DeleteCameraCmd(ICamerasRepository repository)
        {
            Repository = repository;
        }

        protected virtual async Task OnExecuteAsync(IConsole console)
        {
            if (!CameraId.HasValue)
            {
                console.WriteLine("No camera id was specified.");
                return;
            }

            await Repository.DeleteCameraAsync(CameraId.Value);
            console.WriteLine($"Camera with id {CameraId.Value} deleted successfully.");
        }
    }
}