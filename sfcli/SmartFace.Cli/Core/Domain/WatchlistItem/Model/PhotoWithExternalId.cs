using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFace.Cli.Core.Domain.WatchlistItem.Model
{
        public class PhotoWithExternalId
        {
            public bool IsValid { get; }

            public string ExternalId { get; }

            public string PhotoPath { get; }

            public PhotoWithExternalId(bool isValid, string externalId, string photoPath)
            {
                IsValid = isValid;
                ExternalId = externalId;
                PhotoPath = photoPath;
            }
        }
}
