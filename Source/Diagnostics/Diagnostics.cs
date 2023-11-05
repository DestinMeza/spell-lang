using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Spell
{
    public struct Log
    {
        public string message { get; }
        public ELogType ELogType { get; }
        public string memberName { get; }
        public string sourceFilePath { get; }
        public int sourceLineNumber { get; }

        public Log(string _message, ELogType eLogType, string _memberName, string _sourceFilePath, int _sourceLineNumber)
        {
            message = _message;
            ELogType = eLogType;
            memberName = _memberName;
            sourceLineNumber = _sourceLineNumber;
            sourceFilePath = _sourceFilePath;
        }

        public string Message() 
        {
            switch (Diagnostics.DebugType) 
            {
                case EDebugType.Default:
                    return message;
                case EDebugType.Explicit:
                    return $"{message} File: {sourceLineNumber} Line: {sourceFilePath} From: {memberName}";
                case EDebugType.Mute:
                    return "";
                default:
                    return message;
            }
        }
    }

    public enum EDebugType
    {
        Default,
        Mute,
        Explicit,
    }

    public enum ELogType
    {
        Default,
        Warning,
        Error
    }

    public class Diagnostics
    {
        public static EDebugType DebugType => instance.debugType;

        protected static Diagnostics instance;

        protected EDebugType debugType;
        protected List<Log> logs { get; }

        public Diagnostics(EDebugType _debugType = EDebugType.Default)
        {
            if (instance != null)
            {
                LogErrorMessage("Debugger has been created multiple times.");
                return;
            }

            instance = this;

            logs = new List<Log>();
            debugType = _debugType;
        }


        protected virtual bool HiddenAssert(bool state, string errorMessage, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            throw new NotImplementedException();
        }
        protected virtual void HiddenLog(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            throw new NotImplementedException();
        }
        protected virtual void HiddenLogWarning(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            throw new NotImplementedException();
        }
        protected virtual void HiddenLogError(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            throw new NotImplementedException();
        }

        public static bool Assert(bool state, string errorMessage,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            return instance.HiddenAssert(state, errorMessage, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogMessage(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            instance.HiddenLog(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogWarningMessage(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            instance.HiddenLogWarning(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void LogErrorMessage(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            instance.HiddenLogError(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public static EDebugType SetDebugState(EDebugType debugType)
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            instance.debugType = debugType;

            return instance.debugType;
        }

        public static IReadOnlyList<Log> GetLogs()
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            return instance.logs;
        }

        public static void ClearLogs() 
        {
            if (instance == null)
            {
                instance = new DefaultDiagnostics();
            }

            instance.logs.Clear();
        }
    }
}
