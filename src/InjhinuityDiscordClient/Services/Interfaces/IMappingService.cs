using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Dto.Interfaces;
using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IMappingService : IService
    {
        T Map<T>(IDiscordObjectDto data);
        T Map<T>(IEmbedDto data);
    }
}
