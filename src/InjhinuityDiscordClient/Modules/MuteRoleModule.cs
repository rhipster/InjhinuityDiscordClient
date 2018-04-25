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
    public class MuteRoleModule : ListableBotModuleBase<MuteRoleAPIPayload, MuteRoleDto>
    {
        public MuteRoleModule(IBotConfig botConfig, IDiscordModuleService discordModuleService, IEmbedService embedService, 
                                IEmbedPayloadFactory embedPayloadFactory, IResources resources) 
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        [Command("mr update"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Update(IRole role = null)
        {
            if (!await ValidateRole(role, "muterole"))
                return;

            await Put("put", GetDto(Context.Guild.Id, role.Name, role.Id));
        }

        [Command("mr reset"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Reset()
        {
            await Put("reset", GetDto(Context.Guild.Id));
        }

        private async Task<bool> ValidateRole(IRole role, string action)
        {
            var isRoleValid = role != null;

            if (!isRoleValid)
                await HandleCommandResult(action, false, ToList(role?.Name), null, ToList("Role is invalid"));

            return isRoleValid;
        }

        private IDiscordObjectDto GetDto(ulong guildId, string name = "", ulong roleID = 0) =>
            new MuteRoleDto { GuildID = guildId, Name = name, RoleID = roleID };
    }
}
