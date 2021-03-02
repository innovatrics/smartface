using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Commands.SubCamera
{
    [Command(Name = "delete", Description = "Delete a camera")]
    public class DeleteCameraCmd
    {
        [Required]
        [Option("-i|--id", "[Required] Id of camera to delete.", CommandOptionType.SingleValue)]
        public Guid CameraId { get; }

        private ICamerasRepository Repository { get; }

        public DeleteCameraCmd(ICamerasRepository repository)
        {
            Repository = repository;
        }

        protected virtual async Task OnExecuteAsync(IConsole console)
        {
            await Repository.DeleteCameraAsync(CameraId);
            console.WriteLine($"Camera with id {CameraId} deleted successfully.");
        }
    }
}