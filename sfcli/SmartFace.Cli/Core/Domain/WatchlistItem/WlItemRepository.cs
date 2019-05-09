using System.Linq;
using System.Net;
using SmartFace.Api.Rpc;
using SmartFace.Cli.ApiAbstraction;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.Utils;

namespace SmartFace.Cli.Core.Domain.WatchlistItem
{
    public class WlItemRepository : IWlItemRepository
    {
        private IApiDefinition ApiDefinition { get; }
        
        public WlItemRepository(IApiDefinition apiDefinition)
        {
            ApiDefinition = apiDefinition;
        }
        
        public void Register(RegisterWlItemData data)
        {
            Api.Rpc.RegisterWlItemData payload = new Api.Rpc.RegisterWlItemData();
            data.ImageData.ToList().ForEach(imgData => payload.ImageData.Add(new Api.Rpc.RegisterWlItemImageData
            {
                Data = imgData.Data,
                MIME = imgData.MIME
            }));
            data.WatchlistExternalIds.ToList().ForEach(wl => payload.WatchlistExternalIds.Add(wl));
            payload.ExternalId = data.ExternalId;
            
            using (WatchlistItemsRpcClient client = new WatchlistItemsRpcClient(ApiDefinition.ApiUrl))
            {
                var result = client.RegisterAsync(payload).AsyncAwait();
                if (result.StatusCode != (int)HttpStatusCode.NoContent)
                {
                    throw new ProcessingException($"Request end with status code {result.StatusCode}");
                }
            }
        }
    }
}