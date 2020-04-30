using System;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.StreamProcessor;

namespace SmartFace.Cli.Commands.SubVideo
{
    [Command(Name = "add", Description = "Create new video")]
    public class AddVideoCmd : BaseVideoCmd
    {
        public AddVideoCmd(IVideoProcessorRepository repository)
        {
            Repository = repository;
        }

        private IVideoProcessorRepository Repository { get; }

        [Option("--scopeId", "Scope where stream will be created. If empty, new scope will be created.", CommandOptionType.SingleOrNoValue)]
        public (bool HasValue, string Value) ScopeId { get; }

        protected virtual void OnExecute(IConsole console)
        {
            if (!VideoSource.HasValue)
            {
                throw new ProcessingException("VideoSource is required property.");
            }

            var streamProcessor = new VideoProcessor();

            if (ScopeId.HasValue)
            {
                streamProcessor.ScopeId = Guid.Parse(ScopeId.Value);
            }

            SetBaseParameters(streamProcessor);

            var result = Repository.Add(streamProcessor);

            var resultOutput = JsonConvert.SerializeObject(result, Formatting.Indented);
            console.WriteLine(resultOutput);
        }
    }
}