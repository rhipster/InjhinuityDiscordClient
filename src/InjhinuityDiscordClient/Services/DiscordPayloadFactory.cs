using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Services
{
    public class DiscordPayloadFactory : IDiscordPayloadFactory
    {
        private readonly IBotConfig _botConfig;
        private readonly IMappingService _mapper;

        public DiscordPayloadFactory(IBotConfig botConfig, IMappingService mapper)
        {
            _botConfig = botConfig;
            _mapper = mapper;
        }

        public T CreateDiscordObjectPayload<T>(IDiscordObjectDto data) where T : IAPIPayload =>
            _mapper.Map<T>(data);
    }
}
