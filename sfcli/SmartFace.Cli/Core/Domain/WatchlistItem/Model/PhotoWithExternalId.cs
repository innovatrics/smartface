using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFace.Cli.Core.Domain.WatchlistItem.Model
{
        public class PhotoWithExternalId
        {
            public string ExternalId { get; }

            public string PhotoPath { get; }

            public PhotoWithExternalId(string externalId, string photoPath)
            {
                ExternalId = externalId;
                PhotoPath = photoPath;
            }
        }
}
