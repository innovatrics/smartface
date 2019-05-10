using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.OData.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SmartFace.ODataClient.Action;
using SmartFace.ODataClient.Extensions;
using SmartFace.ODataClient.Function;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Infrastructure.ApiClient.Extensions
{
    public static class ODataServiceQueryExtensions
    {
        private class ConfigData
        {
            public string PropertyName { get; set; }
            public object PropertyValue { get; set; }
        }

        public static IDictionary<string, object> GetConfigValuesEx(this DataServiceQuery<Config> service, string name,
            string context)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = service.Context.BaseUri;

                var requestUriPath = service.GetConfigValues(name, context).RequestUri.PathAndQuery;
                var responseMessage = client.GetAsync(requestUriPath).Result;

                var json = responseMessage.Content.ReadAsStringAsync().Result;
                var configValues = JsonConvert.DeserializeObject<IEnumerable<ConfigData>>(json);

                var result = new Dictionary<string, object>();

                foreach (var configValue in configValues)
                {
                    result.Add(configValue.PropertyName, configValue.PropertyValue);
                }

                return result;
            }
        }

        public static Config SetConfigValueEx(this DataServiceQuery<Config> service, string name,
            string context, string propertyName, object propertyValue)
        {
            var collectionCtx = service.Context;
            var setConfigValueUri = service.SetConfigValue(name, context, propertyName, string.Empty).RequestUri;

            var config = collectionCtx.ExecuteAsync<Config>(setConfigValueUri, "POST", true,
                new BodyOperationParameter("Name", name),
                new BodyOperationParameter("Context", context),
                new BodyOperationParameter("Property", propertyName),
                new BodyOperationParameter("Value", propertyValue)).GetAwaiter().GetResult().Single();

            return config;
        }

        public static TEntity Create<TEntity>(this DataServiceQuery<TEntity> service, INotifyPropertyChanged dataObject)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = service.Context.BaseUri;

                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new StringEnumConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                };

                var stringPayload = JsonConvert.SerializeObject(dataObject, settings);

                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var responseMessage = client.PostAsync(service.RequestUri.AbsolutePath, httpContent).Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new ServerException(responseMessage);
                }

                var entity = JsonConvert.DeserializeObject<TEntity>(responseMessage.Content.ReadAsStringAsync().Result);

                return entity;
            }
        }

        public static DataServiceQuery<TEntity> WhereIdIn<TEntity>(this DataServiceQuery<TEntity> service, IEnumerable<long> entityIds)
        {
            var idFilterList = entityIds.Select(id => $"(Id eq {id})");
            var idFilter = string.Join(" or ", idFilterList);
            var query = service.AddQueryOption("$filter", idFilter);
            return query;
        }
    }
}
