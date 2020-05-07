using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    public abstract class Scenario<T> where T : class, new()
    {
        protected delegate void given(T context);
        protected delegate void when(T context);
        protected delegate Task whenAsync(T context);
        protected delegate void then(T context);

        [Test]
        public async Task RunScenario()
        {
            var context = new T();

            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToLookup(x => x.FieldType);
            
            fields[typeof(given)].ToList().ForEach(x => ((given)x.GetValue(this)).Invoke(context));
            fields[typeof(when)].ToList().ForEach(x => ((when)x.GetValue(this)).Invoke(context));
            var whenAsyncTasks = fields[typeof(whenAsync)].Select(wa => ((whenAsync)wa.GetValue(this)).Invoke(context));
            await Task.WhenAll(whenAsyncTasks);
            fields[typeof(then)].ToList().ForEach(x => ((then)x.GetValue(this)).Invoke(context));
        }
    }
}