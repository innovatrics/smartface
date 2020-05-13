using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember
{
    public interface IWatchlistMemberRegistrationManager
    {
        Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(RegisterWatchlistMemberExtended registerWatchlistMemberExtended);

        Task<List<WatchlistMemberWithRelatedData>> RegisterWatchlistMembersFromDirAsync(string directory, string[] watchlistExternalIds,
            int maxDegreeOfParallelism);

        Task<List<WatchlistMemberWithRelatedData>> RegisterWatchlistMembersExtendedFromDirAsync(string directory, string[] watchlistExternalIds,
            int maxDegreeOfParallelism);
    }
}
