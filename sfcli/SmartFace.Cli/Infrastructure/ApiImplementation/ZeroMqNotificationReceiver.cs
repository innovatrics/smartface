using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartFace.Cli.Core.Domain.Notifications;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class ZeroMqNotificationReceiver : INotificationReceiver, IDisposable
    {
        private readonly ZeroMqNotificationReader _reader;

        public ZeroMqNotificationReceiver(ZeroMqNotificationReader reader, ILogger<ZeroMqNotificationReceiver> log)
        {
            _reader = reader;
            _reader.OnError += exception => log.LogError(exception, "Unexpected error during receiving notification");
        }

        public void Start(string topic, TextWriter output)
        {
            if (!_reader.Initialized)
            {
                _reader.OnNotificationReceived += (receivedTopic, json) =>
                {
                    if (receivedTopic != topic)
                    {
                        return;
                    }

                    var formattedString = JToken.Parse(json).ToString(Formatting.Indented);
                    output.WriteLine(formattedString);
                };
                _reader.Init();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //reader is injected and should not be disposed by us ...
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ZeroMqNotificationReceiver()
        {
            Dispose(false);
        }

    }
}
