using System;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Model
{
    public class WatchlistMemberPhotoPath
    {
        public string WatchlistMemberId { get; }

        public string PhotoPath { get; }

        public WatchlistMemberPhotoPath(string watchlistMemberId, string photoPath)
        {
            WatchlistMemberId = watchlistMemberId ?? throw new ArgumentNullException(nameof(watchlistMemberId));
            PhotoPath = photoPath ?? throw new ArgumentNullException(nameof(photoPath));
        }
    }
}
