using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.Domain.ImgExport;
using SmartFace.Cli.Core.Domain.ImgExport.Impl;

namespace SmartFace.Cli.Common.DI.Factories
{
    public class ImageDownloaderFactory : IImageDownloaderFactory
    {
        private IApiDefinition ApiDefinition { get; }
        
        private IServiceProvider ServiceProvider { get; }
        
        public ImageDownloaderFactory(IApiDefinition apiDefinition, IServiceProvider serviceProvider)
        {
            ApiDefinition = apiDefinition;
            ServiceProvider = serviceProvider;
        }

        public IImageDownloader Create(int requestTimeout, string destinationFolder)
        {
            return new ImageDownloader(
                ServiceProvider.GetService<ILogger<ImageDownloader>>(),
                ApiDefinition.OdataBaseUrl, requestTimeout, destinationFolder);
        }

        public void Release(IImageDownloader instance)
        {
            //do nothing
        }
    }
}