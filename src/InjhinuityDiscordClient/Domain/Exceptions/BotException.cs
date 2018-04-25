using Discord.Commands;
using System;

namespace InjhinuityDiscordClient.Domain.Exceptions
{
    public class BotException : Exception
    {
        public ICommandContext Context { get; }
        public string ModuleName { get; }
        public new string StackTrace { get; set; }

        public BotException(ICommandContext context, string moduleName, string exceptionMsg, string stackTrace, Exception inner) : base(exceptionMsg, inner)
        {
            Context = context;
            ModuleName = moduleName;
            StackTrace = stackTrace;
        }
    }
}
