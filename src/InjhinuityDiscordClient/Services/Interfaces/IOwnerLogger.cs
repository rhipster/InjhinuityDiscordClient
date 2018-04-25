using InjhinuityDiscordClient.Domain.Exceptions;
using InjhinuityDiscordClient.Services.Injection;
using System;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IOwnerLogger : IService
    {
        Task SetOwnerDMChannel();
        Task LogException(Exception ex);
        Task LogBotException(BotException ex);
        Task LogError(string errorCode, string extraMsg = "", params string[] args);
    }
}
