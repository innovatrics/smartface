using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using SmartFace.Cli.Core.ApiAbstraction;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public delegate void NotificationReceived(string topic, string json);
    public delegate void ErrorOccured(Exception exception);

    public class ZeroMqNotificationReader : IDisposable
    {
        private readonly string _host;
        private readonly int _port;
        private readonly CancellationTokenSource _source = new CancellationTokenSource();

        private Task _receiveTask;

        public string EndPoint => $">tcp://{_host}:{_port}";
        public bool Initialized { get; private set; }

        public event NotificationReceived OnNotificationReceived;

        public event ErrorOccured OnError;

        public ZeroMqNotificationReader(IApiDefinition apiDefinition)
        {
            _host = apiDefinition.ZeroMqHost;
            _port = apiDefinition.ZeroMqPort;
        }

        public ZeroMqNotificationReader Init()
        {
            if (Initialized)
                throw new InvalidOperationException($"{nameof(ZeroMqNotificationReader)} already initialized.");

            var completionSource = new TaskCompletionSource<object>();
            var token = _source.Token;

            _receiveTask = Task.Factory.StartNew(() =>
            {
                var timeout = TimeSpan.FromSeconds(1);

                using (var subSocket = new SubscriberSocket(EndPoint))
                {
                    try
                    {
                        subSocket.Options.ReceiveHighWatermark = 1000;
                        subSocket.SubscribeToAnyTopic();

                        while (!token.IsCancellationRequested)
                        {
                            var zMessage = new NetMQMessage(2);
                            var messageReceived = subSocket.TryReceiveMultipartMessage(timeout, ref zMessage, 2);
                            completionSource.TrySetResult(null);

                            if (!messageReceived)
                            {
                                continue;
                            }

                            var topic = zMessage.Pop().ConvertToString(Encoding.UTF8);
                            var json = zMessage.Pop().ConvertToString(Encoding.UTF8);

                            OnNotificationReceived?.Invoke(topic, json);
                        }
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }
                }

            }, TaskCreationOptions.LongRunning);

            _receiveTask.ContinueWith(t =>
            {
                // Propagate exception from initialization if occured
                if (t.Exception != null)
                {
                    completionSource.TrySetException(t.Exception);
                }
            });

            completionSource.Task.GetAwaiter().GetResult();

            Initialized = true;
            return this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Signal cancellation
                _source.Cancel();
                // Wait for completion
                _receiveTask?.GetAwaiter().GetResult();
                _source.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ZeroMqNotificationReader()
        {
            Dispose(false);
        }
    }
}
