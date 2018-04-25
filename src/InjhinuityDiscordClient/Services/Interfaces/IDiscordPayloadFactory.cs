using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IDiscordPayloadFactory : IService
    {
        T CreateDiscordObjectPayload<T>(IDiscordObjectDto data) where T : IAPIPayload;
    }
}
