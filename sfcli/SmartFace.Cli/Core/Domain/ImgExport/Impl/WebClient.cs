using System;
using System.Net;

namespace SmartFace.Cli.Core.Domain.ImgExport.Impl
{
    public class WebClient : System.Net.WebClient
    {
        private readonly int _timeout;

        public WebClient(int timeout)
        {
            _timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var w = base.GetWebRequest(uri);
            
            if (w != null)
            {
                w.Timeout = _timeout;
            }

            return w;
        }
    }
}