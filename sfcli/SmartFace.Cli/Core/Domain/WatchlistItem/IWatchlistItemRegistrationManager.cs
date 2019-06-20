using SmartFace.Cli.Core.Domain.WatchlistItem.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistItem
{
    public interface IWatchlistItemRegistrationManager
    {
        void RegisterWlItem(RegisterWlItemExtended registerWlItemExtended);

        void RegisterWlItemsFromDir(string directory, string[] watchlistExternalIds, int maxDegreeOfParallelism);

        void RegisterWlItemsExtendedFromDir(string directory, string[] watchlistExternalIds, int maxDegreeOfParallelism);
    }
}
