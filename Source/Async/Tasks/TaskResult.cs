using System.Threading;

namespace Spell.Async.Tasks
{
    public class TaskResult
    {
        public object Result { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}