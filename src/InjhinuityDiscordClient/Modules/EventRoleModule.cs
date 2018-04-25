using Discord;
using Discord.Commands;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Modules
{
    public class EventRoleModule : ListableBotModuleBase<EventRoleAPIPayload, EventRoleDto>
    {
        public EventRoleModule(IBotConfig botConfig, IDiscordModuleService discordModuleService, IEmbedService embedService, 
                               IEmbedPayloadFactory embedPayloadFactory, IResources resources) 
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        [Command("er update"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Update(IRole role = null)
        {
            if (!await ValidateRole(role, "eventrole"))
                return;

            await Put("put", GetDto(Context.Guild.Id, role.Name, role.Id));
        }

        [Command("er reset"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Reset()
        {
            await Put("reset", GetDto(Context.Guild.Id));
        }

        private async Task<bool> ValidateRole(IRole role, string action)
        {
            var isRoleValid = role != null;

            if (!isRoleValid)
                await HandleCommandResult(action, false, ToList(role?.Name));

            return isRoleValid;
        }

        private IDiscordObjectDto GetDto(ulong guildId, string name = "", ulong roleID = 0) =>
            new EventRoleDto { GuildID = guildId, Name = name, RoleID = roleID };
    }
}
