namespace SmartFace.Cli.Core.Domain.WatchlistMember.Model
{
    public class PhotoWithExternalId
    {
        public string ExternalId { get; }

        public string PhotoPath { get; }

        public PhotoWithExternalId(string externalId, string photoPath)
        {
            ExternalId = externalId;
            PhotoPath = photoPath;
        }
    }
}
