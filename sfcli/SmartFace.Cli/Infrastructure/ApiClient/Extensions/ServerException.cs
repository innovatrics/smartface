using System;
using System.Net.Http;

namespace SmartFace.ODataClient.Extensions
{
    public class ServerException : Exception
    {
        public HttpResponseMessage Response { get; }

        public override string Message => Response.ReasonPhrase;

        public ServerException(HttpResponseMessage response)
        {
            Response = response;
        }

        public override string ToString()
        {
            return Response.Content.ReadAsStringAsync().Result;
        }
    }
}
