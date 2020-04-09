using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember
{
    public interface IWatchlistMemberRegistrationManager
    {
        void RegisterWatchlistMember(RegisterWatchlistMemberExtended registerWatchlistMemberExtended);

        void RegisterWatchlistMembersFromDir(string directory, string[] watchlistExternalIds, int maxDegreeOfParallelism);

        void RegisterWatchlistMembersExtendedFromDir(string directory, string[] watchlistExternalIds, int maxDegreeOfParallelism);
    }
}
