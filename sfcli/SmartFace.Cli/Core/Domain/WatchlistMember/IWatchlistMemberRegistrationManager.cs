using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember
{
    public interface IWatchlistMemberRegistrationManager
    {
        Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(RegisterWatchlistMemberExtended registerWatchlistMemberExtended);

        Task RegisterWatchlistMembersFromDirAsync(string directory, string[] watchlistIds,
            int maxDegreeOfParallelism);

        Task RegisterWatchlistMembersExtendedFromDirAsync(string directory, string[] watchlistIds,
            int maxDegreeOfParallelism);
    }
}
