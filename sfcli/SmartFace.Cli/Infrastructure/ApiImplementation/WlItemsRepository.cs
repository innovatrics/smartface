using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using Microsoft.OData.Client;
using SmartFace.Api.Rpc;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.Utils;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;
using RegisterWlItemData = SmartFace.Cli.Core.ApiAbstraction.Models.RegisterWlItemData;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class WlItemsRepository : IWlItemsRepository
    {
        private Container Container { get; }

        private IApiDefinition ApiDefinition { get; }

        public WlItemsRepository(Container container, IApiDefinition apiDefinition)
        {
            Container = container;
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
                var result = client.RegisterAsync(payload).AwaitSync();
                if (result.StatusCode != (int)HttpStatusCode.NoContent)
                {
                    throw new ProcessingException($"Request end with status code {result.StatusCode}");
                }
            }

            PatchExtendedData(data);
        }

        private void PatchExtendedData(RegisterWlItemData data)
        {
            if (!string.IsNullOrEmpty(data.DisplayName) ||
                !string.IsNullOrEmpty(data.FullName) ||
                !string.IsNullOrEmpty(data.Note))
            {
                var wlItem =
                    ((DataServiceQuery<WlItem>)Container.WatchlistItems.Where(wli => wli.ExternalId == data.ExternalId)
                    ).ExecuteAsync().AwaitSync().ToList().Single();

                WlItemSingle wlItemSingle = Container.WatchlistItems.ByKey(wlItem.Id);
                var patchDelta = new ExpandoObject() as IDictionary<string, object>;
                patchDelta.Add(nameof(WlItem.DisplayName), data.DisplayName);
                patchDelta.Add(nameof(WlItem.FullName), data.FullName);
                patchDelta.Add(nameof(WlItem.Note), data.Note);

                wlItemSingle.PatchPropertyAsync((ExpandoObject)patchDelta).AwaitSync();
            }
        }
    }
}
