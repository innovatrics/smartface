namespace SmartFace.Cli.Core.Domain.WatchlistItem.Model
{
    public class RegisterWlItemExtended
    {
        public string ExternalId { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Note { get; set; }
        public string[] PhotoFiles { get; set; }
        public string[] WatchlistExternalIds { get; set; }
    }
}
