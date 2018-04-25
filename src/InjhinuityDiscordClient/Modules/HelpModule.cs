using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Enums;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Modules
{
    public class HelpModule : BotModuleBase
    {
        private readonly IHelpService _helpService;

        public HelpModule(IHelpService helpService, IBotConfig botConfig, IEmbedService embedService, 
                          IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, embedService, embedPayloadFactory, resources)
        {
            _helpService = helpService;
        }

        [Command("help")]
        public async Task Help()
        {
            var commandInfos = _helpService.GetCommandInfos(((IGuildUser)Context.User).GuildPermissions);
            var descParams = new List<string> { _botConfig.Prefix };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Success, EmbedPayloadType.Help, 
                                                                  "help", Context.User, descParams, null, null, commandInfos);

            var embed = _embedService.CreateFieldEmbed(payload);
            await SendEmbedAsync(embed);
        }
    }
}
