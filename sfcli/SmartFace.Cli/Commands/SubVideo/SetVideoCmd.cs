using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using SmartFace.Cli.Core.Domain.StreamProcessor;

namespace SmartFace.Cli.Commands.SubVideo
{
    [Command(Name = "set", Description = "Edit properties of video")]
    public class SetVideoCmd : BaseVideoCmd
    {
        [Required]
        [Option("-s|--streamId", "Identifier of stream to edit", CommandOptionType.SingleValue)]
        public string StreamId { get; }
        
        private IVideoProcessorRepository Repository { get; }
        
        public SetVideoCmd(IVideoProcessorRepository repository)
        {
            Repository = repository;
        }
        
        protected virtual void OnExecute(IConsole console)
        {
            var streamProcessor = Repository.Read(Guid.Parse(StreamId));

            SetBaseParameters(streamProcessor);

            Repository.Edit(streamProcessor);

            var resultOutput = JsonConvert.SerializeObject(streamProcessor, Formatting.Indented);
            console.WriteLine(resultOutput);
        }
    }
}