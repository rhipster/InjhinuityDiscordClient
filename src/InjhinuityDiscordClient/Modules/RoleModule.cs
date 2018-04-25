using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Modules
{
    public class RoleModule : ListableBotModuleBase<RoleAPIPayload, RoleDto>
    {
        public RoleModule(IDiscordModuleService discordModuleService, IBotConfig botConfig, IEmbedService embedService,
                          IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        [Command("rl create"), Summary("Creates a role. [role]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Create([Remainder] IRole role = null)
        {
            if (!await IsRoleAndPermsValid(role, "role"))
                return;

            await Post("post", GetDto(Context.Guild.Id, role.Name, role.Id));
        }

        [Command("rl delete"), Summary("Deletes a role. [role]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Delete([Remainder] IRole role = null)
        {
            if (!await IsRoleValid(role, "role"))
                return;

            await Delete("delete", GetDto(Context.Guild.Id, role.Name));
        }

        [Command("iam"), Summary("Assigns a role to the user of the command. [role]")]
        public async Task AssignRole([Remainder] IRole role = null)
        {
            if (!await IsRoleAndPermsValid(role, "role"))
                return;

            var result = await Get(GetDto(Context.Guild.Id, role.Name));

            if (result.IsSuccessStatusCode)
            {
                var user = (IGuildUser)Context.User;
                var hasRole = !user.RoleIds.Contains(role.Id);

                await HandleCommandResult("iam", hasRole, ToList(role.Name), null, ToList(""));
                await user.AddRoleAsync(role);

                return;
            }

             await HandleAPIResult(result, "get");
        }

        [Command("iamn"), Summary("Unassigns a role from the user of the command. [role]")]
        public async Task UnassignRole([Remainder] IRole role = null)
        {
            if (!await IsRoleAndPermsValid(role, "role"))
                return;

            var result = await Get(GetDto(Context.Guild.Id, role.Name));

            if (result.IsSuccessStatusCode)
            {
                var user = (IGuildUser)Context.User;
                var hasRole = user.RoleIds.Contains(role.Id);

                await HandleCommandResult("iamn", hasRole, ToList(role.Name), null, ToList(""));
                await user.RemoveRoleAsync(role);

                return;
            }

            await HandleAPIResult(result, "get");
        }

        [Command("rl list"), Summary("Lists every role available for the current guild.")]
        public async Task ListRoles()
        {
            var result = await GetAll(GetDto(Context.Guild.Id));

            if (result.IsSuccessStatusCode)
            {
                CreateAndSendList(result.Content);
                return;
            }

            await HandleAPIResult(result, "get");
        }

        private async Task<bool> IsRoleValid(IRole role, string action)
        {
            var isRoleValid = !IsNull(role);

            if (!isRoleValid)
                await HandleCommandResult(action, false, ToList(role.Name), null, ToList(""));

            return isRoleValid;
        }

        private async Task<bool> IsRoleAndPermsValid(IRole role, string action)
        {
            var isRoleValid = !IsNull(role) && !RoleHasAdminPerm(role);

            if (!isRoleValid)
                await HandleCommandResult(action, false, ToList(role?.Name), null, ToList(""));

            return isRoleValid;
        }

        private IDiscordObjectDto GetDto(ulong guildId, string name = "", ulong roleID = 0) =>
            new RoleDto { GuildID = guildId, Name = name, RoleID = roleID };

        private bool IsNull(IRole role) =>
            role == null;

        private bool RoleHasAdminPerm(IRole role) =>
            role.Permissions.Administrator ||
                role.Permissions.BanMembers ||
                role.Permissions.KickMembers ||
                role.Permissions.ManageChannels ||
                role.Permissions.ManageEmojis ||
                role.Permissions.ManageGuild ||
                role.Permissions.ManageMessages ||
                role.Permissions.ManageNicknames ||
                role.Permissions.ManageRoles ||
                role.Permissions.ManageWebhooks ||
                role.Permissions.ViewAuditLog ||
                role.Permissions.MentionEveryone ||
                role.Permissions.MoveMembers;
    }
}
