using Spell.Async.Interfaces;
using Spell.Async.Tasks;
using System;
using System.Threading.Tasks;

namespace Spell.Async.Tasks
{
    public class AsyncTask<T> : IAsyncTask, ITask where T : class
    {
        public T Result => TaskResult.Result as T;
        public TaskResult TaskResult { get; set; }

        public Task<TaskResult> Task => _task;

        private readonly Task<TaskResult> _task;
        private Func<TaskResult, Task<TaskResult>> _function;

        public AsyncTask(TaskResult taskResult, Task<TaskResult> task = null)
        {
            TaskResult = taskResult;
            _task = task;
            _function = (t) => task;
        }

        public Func<TaskResult, Task<TaskResult>> Function()
        {
            return (TaskResult) => _function.Invoke(TaskResult);
        }

        public void RunAsync() 
        {
            AsyncTaskManager.RunAsync(this);
        }

        public void Forget()
        {
            AsyncTaskVoid asyncTaskVoid = new AsyncTaskVoid(this);

            AsyncTaskManager.Run(asyncTaskVoid);
        }
    }
}