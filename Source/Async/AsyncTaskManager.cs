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
                    Diagnostics.LogWarningMessage("Warning: Async Task Manager was created from a static call and has no direct management. " +
                        "It's recommended to create an Instance of this manually.");
                    instance = new AsyncTaskManager();
                }

                return instance;
            } 
        }

        private static AsyncTaskManager instance;

        public CancellationToken DefaultToken => Instance.tokenSource.Token;

        private CancellationTokenSource tokenSource;

        public AsyncTaskManager(CancellationTokenSource _tokenSource = null) 
        {
            if (instance != null) 
            {
                Diagnostics.LogErrorMessage("AsyncTaskManager instance is expected to be null before constructing another.");
                return;
            }

            if (_tokenSource == null)
            {
                Diagnostics.LogWarningMessage("Warning: A Token Source is recommended to be passed into creating an AsyncTaskManager");
                tokenSource = new CancellationTokenSource();
            }
            else 
            {
                tokenSource = _tokenSource;
            }

            instance = this;
        }

        ~AsyncTaskManager() 
        {
            CancelRootToken();
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

                if (result.Callback != null) 
                {
                    result.Callback.Invoke(result);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This is used mainly for AsyncVoidTask and is expected to use DefaultToken as the CancellationSource.
        /// </summary>
        /// <param name="task"></param>
        internal static void Run(AsyncTaskVoid task) 
        {
            TaskResult taskResult = new TaskResult()
            {
                Result = null,
                CancellationToken = Instance.DefaultToken
            };

            task.Task.TaskResult = taskResult;

            task.TaskFunction.Start();
        }

        public static void CancelRootToken() 
        {
            Instance.tokenSource.Cancel();
            Instance.tokenSource.Dispose();

            Instance.tokenSource = new CancellationTokenSource();
        }
    }
}