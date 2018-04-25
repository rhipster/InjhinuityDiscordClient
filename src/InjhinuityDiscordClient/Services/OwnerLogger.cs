using Discord;
using Discord.WebSocket;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Exceptions;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services
{
    public class OwnerLogger : IOwnerLogger
    {
        private readonly IDiscordClient _client;
        private readonly IEmbedService _embedService;
        private readonly IBotConfig _botConfig;
        private readonly IEmbedPayloadFactory _embedPayloadFactory;
        private readonly IResources _resources;

        private static IDMChannel _ownerDMChannel;

        public OwnerLogger(DiscordSocketClient client, IEmbedService embedService, IBotConfig botConfig,
                            IEmbedPayloadFactory embedPayloadFactory, IResources resources)
        {
            _client = client;
            _embedService = embedService;
            _botConfig = botConfig;
            _embedPayloadFactory = embedPayloadFactory;
            _resources = resources;
        }

        public async Task SetOwnerDMChannel()
        {
            //TODO: Replace hardcoded ID with something a bit cleaner
            var owner = await _client.GetUserAsync(123483031288807424);
            _ownerDMChannel = await owner.GetOrCreateDMChannelAsync();
        }

        public async Task LogException(Exception ex)
        {
            var toLog = ex.InnerException ?? ex;

            var descArgs = new List<string> { toLog.Message };
            var footerArgs = new List<string> { toLog.Source };
            var fields = new List<(string, string, bool)> { (_resources.GetResource("stacktrace_field_title"), toLog.StackTrace, false) };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Exception, EmbedPayloadType.Error, "exception",
                                                                    null, descArgs, null, footerArgs, fields);

            var embed = _embedService.CreateFieldEmbed(payload);
            await _ownerDMChannel.SendEmbedAsync(embed);
        }

        public async Task LogBotException(BotException ex)
        {
            var toLog = ex.InnerException ?? ex;

            var descArgs = new List<string> { toLog.Message, ex.ModuleName };
            var footerArgs = new List<string> { toLog.Source };
            var fields = new List<(string, string, bool)> { (_resources.GetResource("stacktrace_field_title"), toLog.StackTrace, false) };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Exception, EmbedPayloadType.Error, "module_exception",
                                                                    null, descArgs, null, footerArgs, fields);

            var embed = _embedService.CreateFieldEmbed(payload);
            await _ownerDMChannel.SendEmbedAsync(embed);
        }

        public async Task LogError(string errorCode, string extraMsg = "", params string[] args)
        {
            var descArgs = new List<string>(args);
            if (string.IsNullOrEmpty(extraMsg))
                descArgs.Add(extraMsg);

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.ErrorCode, EmbedPayloadType.Error, errorCode, null, descArgs);

            var embed = _embedService.CreateBaseEmbed(payload);
            await _ownerDMChannel.SendEmbedAsync(embed);
        }
    }
}
