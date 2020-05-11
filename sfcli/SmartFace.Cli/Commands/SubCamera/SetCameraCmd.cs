using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Commands.SubCamera
{
    [Command(Name = "set", Description = "Edit properties of a camera")]
    public class SetCameraCmd : BaseCameraModifyingCmd
    {
        [Required]
        [Option("-s|--streamId", "Identifier of camera to edit", CommandOptionType.SingleValue)]
        public Guid StreamId { get; }
        
        private ICamerasRepository Repository { get; }
        
        public SetCameraCmd(ICamerasRepository repository)
        {
            Repository = repository;
        }
        
        protected virtual async Task OnExecuteAsync(IConsole console)
        {
            var dataToUpdate = new CameraRequestData();

            SetBaseParameters(dataToUpdate);

            var updatedCamera = await Repository.UpdateCameraAsync(StreamId, dataToUpdate);

            var resultOutput = JsonConvert.SerializeObject(updatedCamera, Formatting.Indented);
            console.WriteLine(resultOutput);
        }
    }
}