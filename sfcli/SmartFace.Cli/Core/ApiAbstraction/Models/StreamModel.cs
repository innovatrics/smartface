namespace SmartFace.Cli.Core.ApiAbstraction.Models
{
    public class StreamModel
    {
        public long Id { get; set; }
        
        public long ScopeId { get; set; }
        
        public StreamType Type { get; set; }
        
        public StreamState? InputFileState { get; set; }

        public WorkerModel StreamWorker { get; set; }

        public WorkerModel VideoPublishWorker { get; set; }
    }
}