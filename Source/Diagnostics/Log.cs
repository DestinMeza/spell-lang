namespace Spell
{
    public enum ELogType
    {
        Default,
        Warning,
        Error
    }
    public class Log
    {
        public readonly string message;
        public readonly ELogType ELogType;
        public readonly string memberName;
        public readonly string sourceFilePath;
        public readonly int sourceLineNumber;

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
                case EDebugType.Mute:
                    return "";
                case EDebugType.Explicit:
                    return $"\"{message}\" \n File: {sourceFilePath} From: {memberName} Line: {sourceLineNumber}";
                default:
                    return message;
            }
        }
    }
}
