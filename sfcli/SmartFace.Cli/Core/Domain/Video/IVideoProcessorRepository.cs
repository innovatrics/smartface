using System.Collections.Generic;

namespace SmartFace.Cli.Core.Domain.StreamProcessor
{
    public interface IVideoProcessorRepository
    {
        VideoProcessor Read(long id);

        IList<VideoProcessor> ReadAll();

        VideoProcessor Add(VideoProcessor videoProcessor);

        void Edit(VideoProcessor videoProcessor);

        void Delete(VideoProcessor videoProcessor);
    }
}