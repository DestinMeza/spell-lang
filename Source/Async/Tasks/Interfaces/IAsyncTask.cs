using System;
using System.Threading.Tasks;
using Spell.Async.Tasks;

namespace Spell.Async.Interfaces
{
    public interface IAsyncTask
    {
        Func<TaskResult, Task<TaskResult>> Function();
        TaskResult TaskResult { get; set; }
        Task<TaskResult> Task { get; }
    }
}