using Discord;
using Discord.Commands;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Modules
{
    public class LoggingModule : APIBotModuleBase<LogConfigAPIPayload>
    {
        private const int LOG_EVERYTHING_VALUE = 1023;
        private const int LOG_NOTHING_VALUE = 0;

        public LoggingModule(IDiscordModuleService discordModuleService, IBotConfig botConfig, IEmbedService embedService,
                             IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        [Command("log all"), Summary("Enables the logging of every possible event."), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task LogEverything()
        {
            await Put("put", GetDto(Context.Guild.Id, LOG_EVERYTHING_VALUE));
        }

        [Command("log reset"), Summary("Resets the entire logging config back to it's neutral state."), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task LogReset()
        {
            await Put("reset", GetDto(Context.Guild.Id, LOG_NOTHING_VALUE));
        }

        [Command("log add"), Summary("Adds an event for logging. [log_event_name]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task LogAddEvent(string logEvent)
        {
            var result = await Get(GetDto(Context.Guild.Id));

            if (!result.IsSuccessStatusCode)
            {
                await HandleAPIResult(result, "get");
                return;
            }

            void action(LogConfigDto config)
            {
                var paramValue = int.Parse(_resources.GetLogEventValue(logEvent));
                config.LogFlagValue |= 1 << paramValue;
            }

            await ExecutePutAction(action, result);
        }

        [Command("log sub"), Summary("Removes an event from logging. [log_event_name]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task LogRemoveEvent(string logEvent)
        {
            var result = await Get(GetDto(Context.Guild.Id));

            if (!result.IsSuccessStatusCode)
            {
                await HandleAPIResult(result, "reset");
                return;
            }

            void action(LogConfigDto config)
            {
                var paramValue = int.Parse(_resources.GetLogEventValue(logEvent));
                config.LogFlagValue &= ~(1 << paramValue);
            }

            await ExecutePutAction(action, result);
        }

        private async Task ExecutePutAction(Action<LogConfigDto> action, HttpResponseMessage getResult)
        {
            var config = JsonConvert.DeserializeObject<LogConfigDto>(await getResult.Content.ReadAsStringAsync());
            action(config);

            await Put("put", GetDto(config.GuildID, config.LogFlagValue));
        }

        private IDiscordObjectDto GetDto(ulong guildID, int logFlagValue = 0) =>
            new LogConfigDto { GuildID = guildID, LogFlagValue = logFlagValue };
    }
}
