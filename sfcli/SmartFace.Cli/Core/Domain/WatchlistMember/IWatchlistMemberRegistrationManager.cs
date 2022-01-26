using System.Threading;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Commands.SubWatchlistMember;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Core.Domain.WatchlistMember
{
    public interface IWatchlistMemberRegistrationManager
    {
        Task<WatchlistMemberWithRelatedData> RegisterWatchlistMemberAsync(WatchlistMemberRegisterData watchlistMemberRegisterData);
        Task<RegistrationResult> RegisterWatchlistMembersFromDirAsync(string directory, RegisterRequestParams registerRequestParams, int maxDegreeOfParallelism, string fileNameToProperty, CancellationToken cancellationToken);
        Task<RegistrationResult> RegisterWatchlistMembersFromDirByMetadataFileAsync(string directory, RegisterRequestParams registerRequestParams, int maxDegreeOfParallelism, CancellationToken cancellationToken);
    }
}
