using System;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Model
{
    public class WatchlistMemberPhotoPath
    {
        public string WatchlistMemberId { get; init; }

        public string PhotoPath { get; init; }

        public string Name { get; init; }

        public WatchlistMemberPhotoPath()
        {
        }
    }
}
