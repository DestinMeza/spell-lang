using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Spell
{
    /// <summary>
    /// This is used whenever Diagnostics has not be overwritten by the user of the library. This allows for a centralized logging system regardless of Initialization
    /// </summary>
    internal class DefaultDiagnostics : Diagnostics
    {
        public DefaultDiagnostics() : base(EDebugType.Explicit)
        {
        
        }

        protected override bool HiddenAssert(bool state, string errorMessage, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            if (state) 
            {
                HiddenLogError(errorMessage, memberName, sourceFilePath, sourceLineNumber);
            }

            return state;
        }

        protected override void HiddenLog(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            logs.Add(new Log(message, ELogType.Default, memberName, sourceFilePath, sourceLineNumber));
        }

        protected override void HiddenLogError(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            logs.Add(new Log(message, ELogType.Error, memberName, sourceFilePath, sourceLineNumber));
        }

        protected override void HiddenLogWarning(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            logs.Add(new Log(message, ELogType.Warning, memberName, sourceFilePath, sourceLineNumber));
        }
    }
}
