using System;
using System.Collections.Generic;

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

        public Log[] logs => Logs.ToArray();
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

        protected virtual void OnAssert(string errorMessage, string memberName, string sourceFilePath, int sourceLineNumber) 
        {

        }
        protected virtual void OnHiddenLog(string message, string memberName, string sourceFilePath, int sourceLineNumber) 
        {
        
        }
        protected virtual void OnHiddenWarning(string message, string memberName, string sourceFilePath, int sourceLineNumber) 
        { 
        
        }
        protected virtual void OnHiddenLogError(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
        
        }

        public static bool Assert(bool state, string errorMessage, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            if (state) 
            {
                Instance.OnAssert(errorMessage, memberName, sourceFilePath, sourceLineNumber);
            }

            return state;
        }

        public static void LogMessage(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Instance.Logs.Add(new Log(message, ELogType.Default, memberName, sourceFilePath, sourceLineNumber));

            Instance.OnHiddenLog(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogWarningMessage(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Instance.Logs.Add(new Log(message, ELogType.Warning, memberName, sourceFilePath, sourceLineNumber));

            Instance.OnHiddenWarning(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogErrorMessage(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Instance.Logs.Add(new Log(message, ELogType.Error, memberName, sourceFilePath, sourceLineNumber));

            Instance.OnHiddenLogError(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public static EDebugType SetDebugState(EDebugType debugType)
        {
            Instance.debugType = debugType;

            return Instance.debugType;
        }

        public static IReadOnlyList<Log> GetLogs()
        {
            return Instance.Logs;
        }

        public static void ClearLogs() 
        {
            Instance.Logs.Clear();
        }
    }
}
