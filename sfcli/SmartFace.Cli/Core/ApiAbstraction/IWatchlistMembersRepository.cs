using System.Threading.Tasks;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IWatchlistMembersRepository
    {
        Task RegisterAsync(RegisterWatchlistMemberData data);
    }
}
