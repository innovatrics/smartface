using System;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IWatchlistMembersRepository
    {
        Task<WatchlistMemberWithRelatedData> RegisterAsync(RegisterWatchlistMemberData data, Action<RegisterWatchlistMemberRequest> requestModifier);
    }
}
