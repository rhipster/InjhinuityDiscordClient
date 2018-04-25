using Discord.WebSocket;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services
{
    public class BotStartupService : IBotStartupService
    {
        private readonly DiscordSocketClient _client;
        private readonly IOwnerLogger _ownerLogger;
        private readonly IAPIService _apiService;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;

        public BotStartupService(DiscordSocketClient client, IAPIService apiService, IDiscordPayloadFactory discordPayloadFactory,
                                 IOwnerLogger ownerLogger)
        {
            _client = client;
            _apiService = apiService;
            _discordPayloadFactory = discordPayloadFactory;
            _ownerLogger = ownerLogger;
        }

        public async Task SynchroniseGuilds()
        {
            var failedGuilds = new List<string>();

            foreach (var guild in _client.Guilds)
            {
                var dto = new GuildDto { GuildID = guild.Id };
                var payload = _discordPayloadFactory.CreateDiscordObjectPayload<GuildAPIPayload>(dto);

                var result = await _apiService.PostAsync(payload);

                if (!result.IsSuccessStatusCode)
                    failedGuilds.Add(guild.Name);
            }

            if (failedGuilds.Count > 0)
                await _ownerLogger.LogError("0005", failedGuilds.ToString());
        }
    }
}
