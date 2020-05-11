using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class WatchlistMembersRepository : IWatchlistMembersRepository, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly V1Client _v1Client;
        private readonly WatchlistMembersClient _watchlistMembersClient;

        public WatchlistMembersRepository(IApiDefinition apiDefinition, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _v1Client = new V1Client(apiDefinition.ApiUrl, _httpClient);
            _watchlistMembersClient = new WatchlistMembersClient(apiDefinition.ApiUrl, _httpClient);
        }

        public async Task RegisterAsync(RegisterWatchlistMemberData data)
        {
            RegisterWlMemberData payload = new RegisterWlMemberData
            {
                ImageData = data.ImageData.Select(imgData => new ImageData
                {
                    Data = imgData.Data,
                    Mime = imgData.MIME
                }).ToList(),
                WatchlistExternalIds = data.WatchlistExternalIds,
                ExternalId = data.ExternalId
            };

            var watchlistMember = await _watchlistMembersClient.RegisterAsync(payload);

            await UpdateExtendedDataAsync(data, watchlistMember);
        }

        private Task UpdateExtendedDataAsync(RegisterWatchlistMemberData data, WatchlistMemberWithRelatedData newMember)
        {
            if (!string.IsNullOrEmpty(data.DisplayName) ||
                !string.IsNullOrEmpty(data.FullName) ||
                !string.IsNullOrEmpty(data.Note))
            {
                var payload = new WatchlistMemberUpdateData
                {
                    Id = newMember.Id,
                    ExternalId = newMember.ExternalId,

                    DisplayName = data.DisplayName,
                    FullName = data.FullName,
                    Note = data.Note
                };

                return _v1Client.WatchlistMembersPutAsync(payload);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
