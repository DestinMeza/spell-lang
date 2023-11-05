using System;
using System.Threading;

namespace Spell.Async.Tasks
{
    public class TaskResult
    {
        public Action<TaskResult> Callback { get; set; }
        public object Result { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}