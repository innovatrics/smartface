namespace SmartFace.Cli.Core.Domain.WatchlistItem
{
    public interface IWatchlistItemRegistrationManager
    {
        void RegisterWlItem(string wlItemExternalId, string[] watchlistExternalIds, string[] photoPaths);

        void RegisterWlItemsFromDir(string directory, string[] watchlistExternalIds);
    }
}
