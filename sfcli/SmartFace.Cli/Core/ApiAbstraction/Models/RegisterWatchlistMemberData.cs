using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartFace.Cli.Core.ApiAbstraction.Models
{
    public class RegisterWatchlistMemberImageData
    {
        public byte[] Data { get; set; }
    }

    public class RegisterWatchlistMemberData
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
       
        public string FullName { get; set; }

        public string Note { get; set; }

        public ICollection<RegisterWatchlistMemberImageData> ImageData { get; set; } = new Collection<RegisterWatchlistMemberImageData>();

        public ICollection<string> WatchlistIds { get; set; } = new Collection<string>();
    }
}