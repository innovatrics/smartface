namespace SmartFace.Cli.Core.Domain.WatchlistMember.Model
{
    public class WatchlistMemberMetadata
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Note { get; set; }
        public string[] PhotoFiles { get; set; }
        public string[] WatchlistIds { get; set; }
    }
}
