using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.ODataClient.Default;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class ApiProvider : IApiProvider
    {
        private Container Container { get; }
        
        public IWorkersRepository Workers { get; }
        
        public IStreamsRepository Streams { get; }
        
        public IScopesRepository Scopes { get; }
        
        public ICamerasRepository Cameras { get; }
        
        public IStreamWorkerConfigRepository StreamWorkerConfigs { get; }
        
        public IVideoPublishWorkerConfigRepository VideoPublishWorkerConfigs { get; }

        public ApiProvider(Container container, IWorkersRepository workers, IStreamsRepository streams, IScopesRepository scopes, ICamerasRepository cameras, IStreamWorkerConfigRepository streamWorkerConfigs, IVideoPublishWorkerConfigRepository videoPublishWorkerConfigs)
        {
            Container = container;
            Workers = workers;
            Streams = streams;
            Scopes = scopes;
            Cameras = cameras;
            StreamWorkerConfigs = streamWorkerConfigs;
            VideoPublishWorkerConfigs = videoPublishWorkerConfigs;
        }
        
        public void ClearTrackingEntities()
        {
            foreach (var entityDescriptor in Container.EntityTracker.Entities)
            {
                Container.Detach(entityDescriptor.Entity);
            }
            foreach (var link in Container.EntityTracker.Links)
            {
                Container.DetachLink(link.Source, link.SourceProperty, link.Target);
            }
        }
    }
}