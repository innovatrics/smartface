using System;

namespace SmartFace.Cli.Core.ApiAbstraction.Models
{
    public class StreamModel
    {
        public Guid Id { get; set; }
        
        public Guid ScopeId { get; set; }
        
        public StreamType Type { get; set; }
        
        public StreamState? InputFileState { get; set; }

        public WorkerModel StreamWorker { get; set; }

        public WorkerModel VideoPublishWorker { get; set; }
    }
}