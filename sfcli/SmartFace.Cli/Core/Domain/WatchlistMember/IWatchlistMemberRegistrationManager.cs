using System.Threading;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember
{
    public interface IWatchlistMemberRegistrationManager
    {
        Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(WatchlistMemberRegistrationData watchlistMemberRegistrationData);
        Task<RegistrationResult> RegisterWatchlistMembersFromDirAsync(string directory, string[] watchlistIds, int maxDegreeOfParallelism, CancellationToken cancellationToken);
        Task<RegistrationResult> RegisterWatchlistMembersFromDirByMetadataFileAsync(string directory, string[] watchlistIds, int maxDegreeOfParallelism, CancellationToken cancellationToken);
    }
}
