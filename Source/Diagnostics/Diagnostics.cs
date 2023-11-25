using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Spell
{
    public enum EDebugType
    {
        Default,
        Mute,
        Explicit,
    }

    public class Diagnostics
    {
        public static EDebugType DebugType => instance.debugType;

        public static Diagnostics Instance 
        {
            get 
            {
                if (instance == null) 
                {
                    instance = new Diagnostics();
                    LogWarningMessage("Warning: Diagnostics was created from a static call and has no direct management. " +
                        "It's recommended to create an Instance of this manually.");
                }

                return instance;
            }
        }

        private static Diagnostics instance;

        protected EDebugType debugType;
        protected List<Log> Logs { get; set; }

        public Diagnostics(EDebugType _debugType = EDebugType.Default)
        {
            if (instance != null)
            {
                throw new Exception("Diagnostics has been created more than once.");
            }

            instance = this;

            Logs = new List<Log>();
            debugType = _debugType;
        }

        protected virtual void OnAssert(string message, TextSpan span = default) 
        {

        }
        protected virtual void OnHiddenLog(string message, TextSpan span = default) 
        {
        
        }
        protected virtual void OnHiddenWarning(string message, TextSpan span = default) 
        { 
        
        }
        protected virtual void OnHiddenLogError(string message, TextSpan span = default)
        {
        
        }

        public static bool Assert(bool state, string errorMessage, TextSpan span = default)
        {
            if (state) 
            {
                Instance.OnAssert(errorMessage, span);
            }

            return state;
        }

        public static void LogMessage(string message, TextSpan span = default)
        {
            Instance.Logs.Add(new Log(message, span, ELogType.Default));

            Instance.OnHiddenLog(message, span);
        }

        public static void LogWarningMessage(string message, TextSpan span = default)
        {
            Instance.Logs.Add(new Log(message, span, ELogType.Warning));

            Instance.OnHiddenWarning(message, span);
        }

        public static void LogErrorMessage(string message, TextSpan span = default)
        {
            Instance.Logs.Add(new Log(message, span, ELogType.Error));

            Instance.OnHiddenLogError(message, span);
        }

        public static EDebugType SetDebugState(EDebugType debugType)
        {
            Instance.debugType = debugType;

            return Instance.debugType;
        }

        public static Log[] GetLogs()
        {
            Log[] logs = new Log[Instance.Logs.Count];
            for (int i = 0; i < logs.Length; i++) 
            {
                string message = Instance.Logs[i].message;
                TextSpan span = Instance.Logs[i].span;
                ELogType logType = Instance.Logs[i].logType;

                logs[i] = new Log(message, span, logType);
            }

            return logs;
        }

        public static void ClearLogs() 
        {
            Instance.Logs.Clear();
        }
    }
}
