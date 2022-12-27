using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace GoLive.Blazor.Controls
{
    public class ConsoleLogger
    {
        public LogLevel MinimumLoggingLevel { get; set; } = LogLevel.Information;
        
        public void Log(LogLevel level, string Message, [CallerMemberName] string memberName = "",[CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(level, Message,memberName, sourceFilePath, sourceLineNumber);
        }

        private void LogInternal(LogLevel level, string Message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            if (MinimumLoggingLevel > level)
            {
                return;
            }
            Console.WriteLine($"[{DateTime.Now.ToString("O")},{level.ToString().ToUpper()[..1]}] [{sourceFilePath},{sourceLineNumber}] [{memberName}] {Message}");
        }
        
        public void LogTrace(string Message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(LogLevel.Trace, Message, memberName, sourceFilePath, sourceLineNumber);
        }
        
        public void LogDebug(string Message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(LogLevel.Debug, Message, memberName, sourceFilePath, sourceLineNumber);
        }
        
        public void LogInformation(string Message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(LogLevel.Information, Message, memberName, sourceFilePath, sourceLineNumber);
        }
        
        public void LogWarning(string Message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(LogLevel.Warning, Message, memberName, sourceFilePath, sourceLineNumber);
        }
        
        public void LogError(string Message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(LogLevel.Error, Message, memberName, sourceFilePath, sourceLineNumber);
        }
        
        public void LogCritical(string Message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogInternal(LogLevel.Critical, Message, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}