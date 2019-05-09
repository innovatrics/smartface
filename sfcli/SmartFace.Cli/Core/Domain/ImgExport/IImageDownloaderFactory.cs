namespace SmartFace.Cli.Core.Domain.ImgExport
{
    public interface IImageDownloaderFactory
    {
        IImageDownloader Create(int requestTimeout, string destinationFolder);

        void Release(IImageDownloader instance);
    }
}