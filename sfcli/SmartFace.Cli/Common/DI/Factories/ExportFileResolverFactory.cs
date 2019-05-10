using SmartFace.Cli.Core.Domain.ImgExport;
using SmartFace.Cli.Core.Domain.ImgExport.Impl;

namespace SmartFace.Cli.Common.DI.Factories
{
    public class ExportFileResolverFactory : IExportFileResolverFactory
    {
        public IExportFileResolver Create(string resultFileName, string imageUrlProperty)
        {
            return new ExportFileResolver(resultFileName, imageUrlProperty);
        }

        public void Release(IExportFileResolver instance)
        {
            //do nothing
        }
    }
}