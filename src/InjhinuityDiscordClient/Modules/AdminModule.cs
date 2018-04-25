using Discord;
using Discord.Commands;
using Discord.WebSocket;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Modules
{
    public class AdminModule : APIBotModuleBase<MuteRoleAPIPayload>
    {
        private readonly IFormattingService _formattingService;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;

        public AdminModule(IBotConfig botConfig, IEmbedService embedService, IEmbedPayloadFactory embedPayloadFactory, 
                           IResources resources, IFormattingService formattingService, IDiscordPayloadFactory discordPayloadFactory, 
                           IDiscordModuleService discordModuleService) 
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
            _formattingService = formattingService;
            _discordPayloadFactory = discordPayloadFactory;
        }

        [Command("mute"), RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task Mute(string mention)
        {
            var result = await Get(GetDto(Context.Guild.Id));

            if (!result.IsSuccessStatusCode)
            {
                await HandleAPIResult(result, "get");
                return;
            }

            var muteRole = await result.Content.ReadAsJsonAsync<MuteRoleDto>();
            if (muteRole == null || muteRole.RoleID == 0)
            {
                await HandleCommandResult("mute_mia", false, null, null, ToList(""));
                return;
            }

            var user = GetUserFromMention(mention);
            if (user != null && user.GuildPermissions.BanMembers)
            {
                await HandleCommandResult("mute_perm", false, null, null, ToList(""));
                return;
            }

            if (user != null)
                await user.AddRoleAsync(Context.Guild.GetRole(muteRole.RoleID));
            
            await HandleCommandResult("mute", user != null, ToList(user?.FullName()), null, ToList(""));
        }

        [Command("unmute"), RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task Unmute(string mention)
        {
            var result = await Get(GetDto(Context.Guild.Id));

            if (!result.IsSuccessStatusCode)
            {
                await HandleAPIResult(result, "get");
                return;
            }

            var muteRole = await result.Content.ReadAsJsonAsync<MuteRoleDto>();
            if (muteRole == null || muteRole.RoleID == 0)
            {
                await HandleCommandResult("mute_mia", false, null, null, ToList(""));
                return;
            }

            var user = GetUserFromMention(mention);

            if (user != null)
                await user.RemoveRoleAsync(Context.Guild.GetRole(muteRole.RoleID));

            await HandleCommandResult("unmute", user != null, ToList(user?.FullName()), null, ToList(""));
        }

        [Command("ban"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban(string mention, [Remainder]string reason = "")
        {
            var userID = GetUserIDFromMention(mention);
            var user = Context.Guild.GetUser(userID);

            if (user != null && user.GuildPermissions.BanMembers)
            {
                await HandleCommandResult("ban_perm", false, null, null, ToList(""));
                return;
            }

            if (userID != 0 && user != null)
                await Context.Guild.AddBanAsync(userID, 0, reason);

            var display = user?.FullName() ?? userID.ToString();
            await HandleCommandResult("ban", user != null, ToList(display), null, ToList(""));
        }

        [Command("unban"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Unban(string mention)
        {
            var userID = GetUserIDFromMention(mention);
            var bans = await Context.Guild.GetBansAsync();
            var isBanned = bans.Any(x => x.User.Id == userID);

            if (userID != 0 && isBanned)
                await Context.Guild.RemoveBanAsync(userID);

            await HandleCommandResult("unban", isBanned, ToList(userID.ToString()), null, ToList(""));
        }

        [Command("kick"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Kick(string mention, [Remainder]string reason = "")
        {
            var user = GetUserFromMention(mention);

            if (user != null && user.GuildPermissions.BanMembers)
            {
                await HandleCommandResult("kick_perm", false, null, null, ToList(""));
                return;
            }

            if (user != null)
                await user.KickAsync(reason);

            await HandleCommandResult("kick", user != null, ToList(user?.FullName()), null, ToList(""));
        }

        [Command("link"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Link()
        {
            var payload = _embedPayloadFactory.CreateEmbedPayload(Enums.EmbedStruct.Success, Enums.EmbedPayloadType.Info, 
                                                                  "link", Context.Message.Author, new List<string> { _botConfig.BotInviteLink });
            var embed = _embedService.CreateBaseEmbed(payload);

            await SendEmbedAsync(embed);
        }

        private ulong GetUserIDFromMention(string mention)
        {
            var userID = _formattingService.ExtractIDFromMention(mention);
            return ulong.TryParse(userID, out var id) ? id : 0;
        }

        private SocketGuildUser GetUserFromMention(string mention)
        {
            var userID = _formattingService.ExtractIDFromMention(mention);

            return ulong.TryParse(userID, out var id) ?
                Context.Guild.GetUser(id) :
                null;
        }

        private IDiscordObjectDto GetDto(ulong guildID) =>
            new MuteRoleDto { GuildID = guildID };
    }
}
