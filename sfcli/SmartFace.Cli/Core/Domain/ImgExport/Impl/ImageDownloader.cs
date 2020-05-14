using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SmartFace.Cli.Core.Domain.ImgExport.Impl
{
    public class ImageDownloader : IImageDownloader
    {
        private ILogger<ImageDownloader> Log { get; }
        
        private string ApiUrl { get; }
        
        private int RequestTimeout { get; }
        
        private string DestinationFolder { get; }

        public ImageDownloader(ILogger<ImageDownloader> log, string apiUrl, int requestTimeout, string destinationFolder)
        {
            Log = log;
            ApiUrl = apiUrl;
            RequestTimeout = requestTimeout;
            DestinationFolder = destinationFolder;
        }


        public void ExecuteDownload(IEnumerable<ExportFile> resultDefinitions)
        {
            if (!Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }

            foreach (var resultDefinition in resultDefinitions)
            {
                try
                {
                    DownloadFile(resultDefinition);
                }
                catch (Exception e)
                {
                    Log.LogError(e, "Unable to download file.");
                }
            }
        }

        private void DownloadFile(ExportFile exportFile)
        {
            using (var client = new WebClient(RequestTimeout))
            {
                var file = Path.Combine(DestinationFolder, $"{exportFile.FileName}.jpeg");
                var sourceUrl = ApiUrl + "/" + exportFile.ImageUrl;
                Log.LogInformation($"Going to save {sourceUrl} to file {file}");
                client.DownloadFile(new Uri(sourceUrl), file);
                Log.LogInformation("done");
            }
        }
    }
}