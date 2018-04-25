using System;

namespace InjhinuityDiscordClient.Domain.Exceptions
{
    public class ErrorCodeException : Exception
    {
        public string ErrorCode { get; }

        public ErrorCodeException(string errorCode, string exceptionMsg = "", Exception inner = null) : base(exceptionMsg, inner)
        {
            ErrorCode = errorCode;
        }
    }
}
