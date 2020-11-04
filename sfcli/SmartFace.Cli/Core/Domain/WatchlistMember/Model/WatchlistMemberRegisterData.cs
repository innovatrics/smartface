using System;
using SmartFace.Cli.Commands.SubWatchlistMember;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Model
{
    public class WatchlistMemberRegisterData
    {
        public WatchlistMemberMetadata WatchlistMemberMetadata { get; }
        public RegisterRequestParams RegisterRequestParams { get; }

        public WatchlistMemberRegisterData(WatchlistMemberMetadata watchlistMemberMetadata, RegisterRequestParams registerRequestParams)
        {
            WatchlistMemberMetadata = watchlistMemberMetadata ?? throw new ArgumentNullException(nameof(watchlistMemberMetadata));
            RegisterRequestParams = registerRequestParams ?? throw new ArgumentNullException(nameof(registerRequestParams));
        }
    }
}
