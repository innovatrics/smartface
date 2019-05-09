namespace SmartFace.Cli.Core.Domain.ImgExport
{
    public interface IExportFileResolverFactory
    {
        IExportFileResolver Create(string resultFileName, string imageUrlProperty);

        void Release(IExportFileResolver instance);
    }
}