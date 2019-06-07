using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.Utils;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.Core.Domain.StreamProcessor
{
    public class VideoProcessorRepository : IVideoProcessorRepository
    {
        private IApiProvider ApiProvider { get; }
        
        private IMapper Mapper { get; }

        private ILogger<VideoProcessorRepository> Log { get; }


        public VideoProcessorRepository(IApiProvider apiProvider, ILogger<VideoProcessorRepository> log, IMapper mapper)
        {
            ApiProvider = apiProvider;
            Log = log;
            Mapper = mapper;
        }

        public IList<VideoProcessor> ReadAll()
        {
            var resultList = new List<VideoProcessor>();
            var streams = ApiProvider.Streams.Get().AwaitSync();
            streams.ToList().ForEach(s => resultList.Add(ReadInternal(s)));
            return resultList;
        }

        public VideoProcessor Read(long id)
        {
            var stream = GetStreamById(id);
            return ReadInternal(stream);
        }


        private StreamModel GetStreamById(long streamId)
        {
            return ApiProvider.Streams.Get(streamId).AwaitSync();
        }


        private bool IsClosed(StreamModel stream)
        {
            return stream.StreamWorker == null && stream.VideoPublishWorker == null;
        }

        private VideoProcessor ReadInternal(StreamModel stream)
        {
            if (IsClosed(stream))
            {
                return new VideoProcessor {StreamId = stream.Id};
            }

            var type = stream.Type;

            var streamWorker = stream.StreamWorker;
            var videoWorker = stream.VideoPublishWorker;

            var enabled = videoWorker.Enabled && streamWorker.Enabled;

            StreamState state;
            switch (type)
            {
                case StreamType.LiveCamera:
                    state = enabled ? StreamState.Processing : StreamState.Ready;
                    break;
                case StreamType.File:
                    state = stream.InputFileState.Value;
                    break;
                default:
                    throw new NotSupportedException($"{nameof(StreamType)} {type}");
            }


            var streamConfig = ApiProvider.StreamWorkerConfigs.Get(streamWorker.Id);
            var videoConfig = ApiProvider.VideoPublishWorkerConfigs.Get(videoWorker.Id);

            var processor = new VideoProcessor
            {
                ScopeId = stream.ScopeId,
                Enabled = enabled,
                StreamId = stream.Id,
                Type = type,
                State = state
            };

            Mapper.Map(streamConfig, processor);
            Mapper.Map(videoConfig, processor);
            
            return processor;
        }

        public VideoProcessor Add(VideoProcessor videoProcessor)
        {
            bool createScope = false;
            if (videoProcessor.StreamId != 0)
            {
                throw new ProcessingException($"Unable to set readonly property {nameof(videoProcessor.StreamId)}");
            }

            if (videoProcessor.Enabled.HasValue && videoProcessor.Enabled.Value && string.IsNullOrEmpty(videoProcessor.VideoSource))
            {
                throw new ProcessingException(
                    $"Unable to enable {nameof(VideoProcessor)}, {nameof(videoProcessor.VideoSource)} is empty");
            }

            if (!videoProcessor.ScopeId.HasValue)
            {
                createScope = true;
                Log.LogWarning($"Property {nameof(videoProcessor.ScopeId)} is not set. New Scope will be created.");
            }

            if (createScope)
            {
                var scope = ApiProvider.Scopes.Create();
                videoProcessor.ScopeId = scope.Id;
            }

            var createCameraModel = new CreateCameraModel
            {
                ScopeId = videoProcessor.ScopeId.Value,
                VideoSource = videoProcessor.VideoSource
            };
            var camera = ApiProvider.Cameras.Create(createCameraModel);

            videoProcessor.StreamId = camera.StreamId;
            Edit(videoProcessor);
            videoProcessor = Read(camera.StreamId);

            return videoProcessor;
        }

        public void Edit(VideoProcessor videoProcessor)
        {
            var stream = GetStreamById(videoProcessor.StreamId);

            if (IsClosed(stream))
            {
                return;
            }

            var streamWorker = stream.StreamWorker;
            var videoWorker = stream.VideoPublishWorker;

            var videoConfig = Mapper.Map<IVideoPublishWorkerConfig, VideoPublishWorkerConfigModel>(videoProcessor);
            ApiProvider.VideoPublishWorkerConfigs.Set(videoWorker.Id, videoConfig);
            
            var streamConfig = Mapper.Map<IStreamWorkerConfig, StreamWorkerConfigModel>(videoProcessor);
            ApiProvider.StreamWorkerConfigs.Set(streamWorker.Id, streamConfig);

            bool isEnabled = videoWorker.Enabled && streamWorker.Enabled;
            if (videoProcessor.Enabled.HasValue && videoProcessor.Enabled.Value != isEnabled)
            {
                if (videoProcessor.Enabled.Value)
                {
                    ApiProvider.Workers.EnableWorker(videoWorker.Id).AwaitSync();
                    ApiProvider.Workers.EnableWorker(streamWorker.Id).AwaitSync();
                }
                else
                {
                    ApiProvider.Workers.DisableWorker(videoWorker.Id).AwaitSync();
                    ApiProvider.Workers.DisableWorker(streamWorker.Id).AwaitSync();
                }
            }

            ApiProvider.ClearTrackingEntities();
            var updatedStream = GetStreamById(stream.Id);
            var updatedStreamProcessor = ReadInternal(updatedStream);
            updatedStreamProcessor.CopyProperties(videoProcessor);
        }

        public void Delete(VideoProcessor videoProcessor)
        {
            throw new NotImplementedException();
        }
    }
}