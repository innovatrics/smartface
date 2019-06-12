using System.Threading.Tasks;

namespace SmartFace.Cli.Common.Utils
{
    public static class TaskExtensions
    {
        public static T AwaitSync<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        public static void AwaitSync(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

    }
}