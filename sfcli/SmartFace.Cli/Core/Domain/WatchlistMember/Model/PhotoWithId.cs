namespace SmartFace.Cli.Core.Domain.WatchlistMember.Model
{
    public class PhotoWithId
    {
        public string Id { get; }

        public string PhotoPath { get; }

        public PhotoWithId(string id, string photoPath)
        {
            Id = id;
            PhotoPath = photoPath;
        }
    }
}
