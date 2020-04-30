using System;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using SmartFace.Cli.Core.Domain.StreamProcessor;

namespace SmartFace.Cli.Commands.SubVideo
{
    [Command(Name = "get", Description = "Read properties of video")]
    public class GetVideoCmd
    {

        [Option("-s|--streamId", "Identifier of stream to edit", CommandOptionType.SingleValue)]
        public (bool HasValue, string Value) StreamId { get; }
        
        private IVideoProcessorRepository Repository { get; }
        
        public GetVideoCmd(IVideoProcessorRepository repository)
        {
            Repository = repository;
        }
        
        protected virtual void OnExecute(IConsole console)
        {
            object result = StreamId.HasValue ? (object) Repository.Read(Guid.Parse(StreamId.Value)) : Repository.ReadAll();
            var resultOutput = JsonConvert.SerializeObject(result, Formatting.Indented);
            console.WriteLine(resultOutput);
        }
    }
}