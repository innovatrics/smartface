using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.ImgExport;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    [Command(Name = "exportimage", Description = "Exports image from entity \"ImageUrl\" property to file.")]
    public class ExportImage
    {
        [Required]
        [Option("-f|--format",Description = "Define placeholders for file name builder. Use property name in braces. For example \"trackletId_{Tracklet.Faces.Id}\" will result as file \"trackletId_3244.jpeg\"")]
        public string Format { get; }

        [Required]
        [Option("-d|--destination", Description = "Destination folder")]
        public string DestinationFolder { get; }

        [Required]
        [Option("-i|--imageUrlProp", Description = "Property path to image url on given object. E.g. \"Tracklet.Faces.ImageUrl\" or \"Frame.ImageUrl\"")]
        public string ImageUrlProperty { get; }

        [Option("-t|--timeout", Description = "Length of time, in milliseconds, before the request times out")]
        public int RequestTimeout { get; } = 2000;
        
        protected IFromCmd Parent { get; set; }

        private ILogger<ExportImage> Log { get; }

        
        private IImageDownloaderFactory ImageDownloaderFactory { get; }
        
        private IExportFileResolverFactory ExportFileResolverFactory { get; }

        public ExportImage(ILogger<ExportImage> log, IImageDownloaderFactory imageDownloaderFactory, IExportFileResolverFactory exportFileResolverFactory)
        {
            Log = log;
            ImageDownloaderFactory = imageDownloaderFactory;
            ExportFileResolverFactory = exportFileResolverFactory;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            var resolver = ExportFileResolverFactory.Create(Format, ImageUrlProperty);
            var downloader = ImageDownloaderFactory.Create(RequestTimeout, DestinationFolder);

            var entities = Parent.Execute(console);
            var exportFiles = resolver.Resolve(entities);
            downloader.ExecuteDownload(exportFiles);
            
            ExportFileResolverFactory.Release(resolver);
            ImageDownloaderFactory.Release(downloader);
            return Constants.EXIT_CODE_OK;
        }
    }
}