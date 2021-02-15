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

        public async Task<WatchlistMemberWithRelatedData> RegisterAsync(RegisterWatchlistMemberData data, Action<RegisterWatchlistMemberRequest> requestModifier)
        {
            var requestPayload = new RegisterWatchlistMemberRequest
            {
                Images = data.ImageData.Select(imgData => new RegistrationImageData
                {
                    Data = imgData.Data
                }).ToList(),
                WatchlistIds = data.WatchlistIds,
                Id = data.Id
            };

            requestModifier?.Invoke(requestPayload);

            var watchlistMember = await _watchlistMembersClient.RegisterAsync(requestPayload);

            return await UpdateExtendedDataAsync(data, watchlistMember);
        }

        private async Task<WatchlistMemberWithRelatedData> UpdateExtendedDataAsync(RegisterWatchlistMemberData data,
            WatchlistMemberWithRelatedData newMember)
        {
            if (!string.IsNullOrEmpty(data.DisplayName) ||
                !string.IsNullOrEmpty(data.FullName) ||
                !string.IsNullOrEmpty(data.Note))
            {
                var payload = new WatchlistMemberUpsertRequest
                {
                    Id = newMember.Id,
                    DisplayName = data.DisplayName,
                    FullName = data.FullName,
                    Note = data.Note
                };

                var updatedWatchlistMember = await _v1Client.WatchlistMembersPutAsync(payload);

                newMember.DisplayName = updatedWatchlistMember.DisplayName;
                newMember.FullName = updatedWatchlistMember.FullName;
                newMember.Note = updatedWatchlistMember.Note;
            }

            return newMember;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
