using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class StreamsRepository : IStreamsRepository
    {
        private Container Container { get; }
        
        public StreamsRepository(Container container)
        {
            Container = container;
        }
        
        private DataServiceQuery<Stream> BuildQuery()
        {
            var query = Container.Streams
                .Expand("Cameras($expand=StreamWorker)")
                .Expand("Cameras($expand=VideoPublishWorker)")
                .Expand("InputFiles($expand=StreamWorker)")
                .Expand("InputFiles($expand=VideoPublishWorker)");
            return query;
        }
        
        public async Task<StreamModel> Get(Guid id)
        {
            var streams = await BuildQuery().WhereIdIn(new[] {id}).ExecuteAsync();
            return StreamModelFromStream(streams.Single());
        }

        public async Task<IEnumerable<StreamModel>> Get()
        {
            var streams = await BuildQuery().ExecuteAsync();
            return streams.Select(StreamModelFromStream);
        }

        private StreamModel StreamModelFromStream(Stream stream)
        {
            var streamWorker = GetStreamWorker(stream);
            var videoWorker = GetVideoWorker(stream);

            var streamWorkerModel = streamWorker.ToDomainModel();
            var videoWorkerModel = videoWorker.ToDomainModel();

            var inputFile = stream.InputFiles.SingleOrDefault();
            StreamType type = inputFile != null ? StreamType.File : StreamType.LiveCamera;
            StreamState? state = inputFile != null ? (StreamState?)(int)inputFile.State : (StreamState?)null;
            
            return new StreamModel
            {
                Id = stream.Id,
                ScopeId = stream.ScopeId.Value,
                StreamWorker = streamWorkerModel,
                VideoPublishWorker = videoWorkerModel,
                Type = type,
                InputFileState = state
            };
        }

        
        private Worker GetStreamWorker(Stream stream)
        {
            return stream.InputFiles?.SingleOrDefault()?.StreamWorker ??
                   stream.Cameras?.SingleOrDefault()?.StreamWorker;
        }

        private Worker GetVideoWorker(Stream stream)
        {
            return stream.InputFiles?.SingleOrDefault()?.VideoPublishWorker ??
                   stream.Cameras?.SingleOrDefault()?.VideoPublishWorker;
        }
    }
}