using SmartFace.Cli.Core.Domain.WatchlistItem.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistItem
{
    public interface IWatchlistItemRegistrationManager
    {
        void RegisterWlItem(RegisterWlItemExtended registerWlItemExtended, string[] watchlistExternalIds);

        void RegisterWlItemsFromDir(string directory, string[] watchlistExternalIds);

        void RegisterWlItemsExtendedFromDir(string directory, string[] watchlistExternalIds);
    }
}
