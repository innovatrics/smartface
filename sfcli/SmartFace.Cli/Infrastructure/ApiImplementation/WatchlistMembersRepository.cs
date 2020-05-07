using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class WatchlistMembersRepository : IWatchlistMembersRepository
    {
        private IApiDefinition ApiDefinition { get; }

        public WatchlistMembersRepository(IApiDefinition apiDefinition)
        {
            ApiDefinition = apiDefinition;
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

            using var httpClient = new HttpClient();
            var client = new WatchlistMembersClient(ApiDefinition.ApiUrl, httpClient);
            var watchlistMember = await client.RegisterAsync(payload);

            await UpdateExtendedDataAsync(data, new V1Client(ApiDefinition.ApiUrl, httpClient), watchlistMember);
        }

        private Task UpdateExtendedDataAsync(RegisterWatchlistMemberData data, V1Client client, WatchlistMemberWithRelatedData newMember)
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

                return client.WatchlistMembersPutAsync(payload);
            }

            return Task.CompletedTask;
        }
    }
}
