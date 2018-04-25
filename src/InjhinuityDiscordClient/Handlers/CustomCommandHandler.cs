using Discord.Commands;
using System.Threading.Tasks;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Services.Injection;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Extensions;

namespace InjhinuityDiscordClient.Handlers
{
    public class CustomCommandHandler : IService
    {
        private readonly IAPIService _apiService;
        private readonly IBotConfig _botConfig;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;

        public CustomCommandHandler(IAPIService apiService, IBotConfig botConfig, IDiscordPayloadFactory discordPayloadFactory)
        {
            _apiService = apiService;
            _botConfig = botConfig;
            _discordPayloadFactory = discordPayloadFactory;
        }

        public async Task<bool> TryHandlingCustomCommand(ICommandContext context, string message)
        {
            if (message[0] != char.Parse(_botConfig.Prefix))
                return false;

            string cleanMsg = message.Substring(1, message.Length - 1);

            var command = new CommandDto { GuildID = context.Guild.Id, Name = cleanMsg };
            var payload = _discordPayloadFactory.CreateDiscordObjectPayload<CommandAPIPayload>(command);

            var result = await _apiService.GetAsync(payload);

            if (result.IsSuccessStatusCode)
            {
                var resultCommand = await result.Content.ReadAsJsonAsync<CommandDto>();
                await context.Channel.SendMessageAsync(resultCommand.Body);
                return true;
            }

            return false;
        }
    }
}
