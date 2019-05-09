using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace SmartFace.Api.Rpc
{
    public partial class WatchlistItemsRpcClient : IDisposable
    {
        private readonly bool _httpClientIsInternalResource;

        public WatchlistItemsRpcClient(string url)
        {
            _httpClientIsInternalResource = true;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            BaseUrl = url;

            _settings = new Lazy<JsonSerializerSettings>(() =>
            {
                var settings = new JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpClientIsInternalResource)
                {
                    _httpClient?.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WatchlistItemsRpcClient()
        {
            Dispose(false);
        }
    }
}
