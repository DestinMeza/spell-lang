using Spell.Async.Interfaces;

using System;
using System.Threading.Tasks;

namespace Spell.Async.Tasks
{
    public class AsyncTaskVoid : ITask
    {
        public Task<Func<TaskResult, Task<TaskResult>>> TaskFunction { get; set; }
        public IAsyncTask Task { get; set; }

        public AsyncTaskVoid(IAsyncTask task)
        {
            Task = task;
            TaskFunction = new Task<Func<TaskResult, Task<TaskResult>>>(task.Function);
        }

        public void Forget()
        {
            AsyncTaskManager.Run(this);
        }
    }
}