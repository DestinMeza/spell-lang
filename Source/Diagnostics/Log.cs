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
        public readonly TextSpan span;
        public readonly ELogType logType;

        public Log(string _message, TextSpan _span, ELogType _logType)
        {
            message = _message;
            span = _span;
            logType = _logType;
        }

        public string Message() 
        {
            switch (Diagnostics.DebugType) 
            {
                case EDebugType.Default:
                    return message;
                case EDebugType.Mute:
                    return "";
                default:
                    return message;
            }
        }
    }
}
