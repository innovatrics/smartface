using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartFace.Cli.Core.Domain.WatchlistItem
{
    public class RegisterWlItemImageData
    {
        public byte[] Data { get; set; }

        public string MIME { get; set; }
    }
    
    public class RegisterWlItemData
    {
        public string ExternalId { get; set; }

        public ICollection<RegisterWlItemImageData> ImageData { get; set; } = new Collection<RegisterWlItemImageData>();

        public ICollection<string> WatchlistExternalIds { get; set; } = new Collection<string>();
    }
}