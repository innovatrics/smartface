using System.Threading.Tasks;

namespace SmartFace.Cli.Common.Utils
{
    public static class TaskExtensions
    {
        public static T AsyncAwait<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}