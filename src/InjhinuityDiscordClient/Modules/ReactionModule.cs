using Discord;
using Discord.Commands;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Modules
{
    public class ReactionModule : ListableBotModuleBase<ReactionAPIPayload, ReactionDto>
    {
        public ReactionModule(IBotConfig botConfig, IDiscordModuleService discordModuleService, IEmbedService embedService, 
                              IEmbedPayloadFactory embedPayloadFactory, IResources resources) 
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        [Command("rc create"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Create()
        {

        }
    }
}
