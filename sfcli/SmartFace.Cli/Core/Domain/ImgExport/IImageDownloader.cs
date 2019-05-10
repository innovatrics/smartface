using System.Collections.Generic;

namespace SmartFace.Cli.Core.Domain.ImgExport
{
    public interface IImageDownloader
    {
        void ExecuteDownload(IEnumerable<ExportFile> resultDefinitions);
    }
}