using System;
using System.Threading;
using System.Threading.Tasks;
using Spell;
using Spell.Async.Tasks;
using Spell.Async.Interfaces;
using System.Collections.Generic;

namespace Spell.Async
{
    /// <summary>
    /// This is used for managing all GameTask in a game which are Async Operations that allow for ease of multi threaded operations
    /// </summary>
    public class AsyncTaskManager
    {
        public static AsyncTaskManager Instance { get 
            {
                if (instance == null) 
                {
                    instance = new AsyncTaskManager();
                }

                return instance;
            } 
        }

        public static CancellationToken DefaultToken => TokenSource.Token;

        private static AsyncTaskManager instance;
        private static CancellationTokenSource TokenSource;

        public AsyncTaskManager() 
        {
            if (instance != null) 
            {
                Diagnostics.LogErrorMessage("AsyncTaskManager static instance is expected to be null before constructing another.");
                return;
            }

            TokenSource = new CancellationTokenSource();
            instance = this;
        }

        public static async Task<TaskResult> RunAsync(IAsyncTask asyncTask)
        {
            if (asyncTask.Task == null) 
            {
                return await asyncTask.Task;
            }

            try
            {
                async Task<TaskResult> asyncFunction(TaskResult t) => await asyncTask.Function().Invoke(t);

                var result = await asyncFunction(asyncTask.TaskResult);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Run(AsyncTaskVoid task, CancellationToken ct) 
        {
            TaskResult taskResult = new TaskResult()
            {
                Result = false,
                CancellationToken = ct
            };

            task.Task.TaskResult = taskResult;

            task.TaskFunction.Start();
        }

        public static void CancelAllForgottenTask() 
        {
            TokenSource.Cancel();
            TokenSource.Dispose();

            TokenSource = new CancellationTokenSource();
        }
    }
}