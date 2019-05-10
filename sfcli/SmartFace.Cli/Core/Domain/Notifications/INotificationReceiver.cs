using System.IO;

namespace SmartFace.Cli.Core.Domain.Notifications
{
    public interface INotificationReceiver
    {
        void Start(string topic, TextWriter output);
    }
}
