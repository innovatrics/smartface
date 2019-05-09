namespace SmartFace.Cli.Core.Domain.ImgExport
{
    public class ExportFile
    {
        public string FileName { get; }
        public string ImageUrl { get; }

        public ExportFile(string fileName, string imageUrl)
        {
            FileName = fileName;
            ImageUrl = imageUrl;
        }
    }
}