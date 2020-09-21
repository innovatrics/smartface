using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.Notifications;
using SmartFace.Cli.Infrastructure.ApiClient.Notifications;

namespace SmartFace.Cli.Commands
{
    [Command(Name = "notifications", Description = "Receive and print notifications to console")]
    public class NotificationsCmd
    {
        [Required]
        [Option("-t|--topic", "Specify topic of notifications ["
                              + ZeroMqNotificationTopic.FACE_CREATED + ", "
                              + ZeroMqNotificationTopic.FACE_EXTRACTED + ", "
                              + ZeroMqNotificationTopic.GROUPING_PROGRESS_INFO + ", "
                              + ZeroMqNotificationTopic.VIDEO_RECORD_STATE_UPDATE + ", "
                              + ZeroMqNotificationTopic.TRACKLET_COMPLETED + ", "
                              + ZeroMqNotificationTopic.MATCH_RESULT_MATCH + ", "
                              + ZeroMqNotificationTopic.MATCH_RESULT_NO_MATCH + ", "
                              + ZeroMqNotificationTopic.MATCH_RESULT_MATCH_INSERTED + ", "
                              + ZeroMqNotificationTopic.LIVENESS_RESULT + ", "
                              + ZeroMqNotificationTopic.HEARTBEAT  + ", "
                              + ZeroMqNotificationTopic.PEDESTRIANS_INSERTED
                              + "]", CommandOptionType.SingleValue)]
        [AllowedValues(
            ZeroMqNotificationTopic.FACE_CREATED,
            ZeroMqNotificationTopic.FACE_EXTRACTED,
            ZeroMqNotificationTopic.GROUPING_PROGRESS_INFO,
            ZeroMqNotificationTopic.VIDEO_RECORD_STATE_UPDATE,
            ZeroMqNotificationTopic.TRACKLET_COMPLETED,
            ZeroMqNotificationTopic.MATCH_RESULT_MATCH,
            ZeroMqNotificationTopic.MATCH_RESULT_NO_MATCH,
            ZeroMqNotificationTopic.MATCH_RESULT_MATCH_INSERTED,
            ZeroMqNotificationTopic.LIVENESS_RESULT,
            ZeroMqNotificationTopic.HEARTBEAT,
            ZeroMqNotificationTopic.PEDESTRIANS_INSERTED
        , IgnoreCase = false)]
        public string Topic { get; set; }

        private INotificationReceiver Receiver { get; }
        public NotificationsCmd(INotificationReceiver receiver)
        {
            Receiver = receiver;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("Press ENTER to exit");
            Receiver.Start(Topic, console.Out);
            console.In.ReadLine();
            return Constants.EXIT_CODE_OK;
        }
    }
}