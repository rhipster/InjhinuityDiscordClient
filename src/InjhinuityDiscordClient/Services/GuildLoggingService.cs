using Discord;
using Discord.WebSocket;
using InjhinuityDiscordClient.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using InjhinuityDiscordClient.Extensions;
using System.Linq;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Services
{
    public class GuildLoggingService : IGuildLoggingService
    {
        private readonly DiscordSocketClient _client;
        private readonly IAPIService _apiService;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;
        private readonly IEmbedPayloadFactory _embedPayloadFactory;
        private readonly IResources _resourceService;
        private readonly IEmbedService _embedService;

        public GuildLoggingService(DiscordSocketClient client, IAPIService apiService, IDiscordPayloadFactory discordPayloadFactory,
                              IEmbedPayloadFactory embedPayloadFactory, IResources resourceService, IEmbedService embedService)
        {
            _client = client;
            _apiService = apiService;
            _discordPayloadFactory = discordPayloadFactory;
            _embedPayloadFactory = embedPayloadFactory;
            _resourceService = resourceService;
            _embedService = embedService;

            client.UserJoined += UserJoined;
            client.UserLeft += UserLeft;
            client.UserBanned += UserBanned;
            client.UserUnbanned += UserUnbanned;
            client.UserUpdated += UserUpdated;
            client.MessageUpdated += MessageUpdated;
            client.MessageDeleted += MessageDeleted;
        }

        private async Task MessageDeleted(Cacheable<IMessage, ulong> msg, ISocketMessageChannel msgChannel)
        {
            var deletedMsg = msg.Value;

            if (IsMsgNull(deletedMsg) || IsUserNull(deletedMsg.Author) || 
                IsUserBot(deletedMsg.Author) || IsMsgEmpty(deletedMsg))
                return;

            var channel = msgChannel as SocketGuildChannel;

            var logChannel = await TryGetLoggingChannel(channel.Guild.Id, EventType.MessageDeleted, deletedMsg.Author, channel.Guild);
            if (logChannel == null)
                return;

            var descParams = new List<string> { deletedMsg.Id.ToString(), deletedMsg.Author.FullName(), deletedMsg.Author.Id.ToString() };
            var fields = new List<(string, string, bool)> {{ ( _resourceService.GetResource("msg_deleted_field_title"), deletedMsg.Content, false) }};

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.MsgDeleted, "msg_deleted", 
                                                                    deletedMsg.Author, descParams, null, null, fields);

            var embed = _embedService.CreateFieldEmbed(payload);
            await channel.Guild
                .GetTextChannel(logChannel.ChannelID)
                .SendEmbedAsync(embed);
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> msg, SocketMessage newMsg, ISocketMessageChannel msgChannel)
        {
            var oldMsg = msg.Value;

            if (IsMsgNull(newMsg) || IsMsgNull(oldMsg) || IsUserNull(newMsg.Author) || 
                IsUserBot(newMsg.Author) || IsMsgEmpty(oldMsg) || IsMsgEmpty(newMsg))
                return;

            var channel = msgChannel as SocketGuildChannel;

            var logChannel = await TryGetLoggingChannel(channel.Guild.Id, EventType.MessageUpdated, newMsg.Author, channel.Guild);
            if (logChannel == null)
                return;

            var descParams = new List<string> { newMsg.Id.ToString(), newMsg.Author.FullName(), newMsg.Author.Id.ToString() };
            var fields = new List<(string, string, bool)>
            {
                { ( _resourceService.GetResource("msg_old_field_title"), oldMsg.Content, false) },
                { ( _resourceService.GetResource("msg_new_field_title"), newMsg.Content, false) }
            };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.MsgUpdated, "msg_updated", 
                                                                    newMsg.Author, descParams, null, null, fields);

            var embed = _embedService.CreateFieldEmbed(payload);
            await channel.Guild
                .GetTextChannel(logChannel.ChannelID)
                .SendEmbedAsync(embed);
        }

        private async Task UserUpdated(SocketUser user, SocketUser newUser)
        {
            if (IsUserNull(user) || IsUserNull(newUser))
                return;

            var guildConfigs = await GetAllGuildConfigs();

            if (user.Username != newUser.Username)
            {
                foreach (SocketGuild guild in _client.Guilds)
                {
                    if (!guild.Users.Contains(newUser))
                        continue;

                    var channel = await TryGetLoggingChannel(guild.Id, EventType.UsernameUpdated, newUser, guild);
                    if (channel == null)
                        continue;

                    var descParams = new List<string> { user.FullName(), newUser.FullName() };

                    var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.UserUpdated, "user_updated_username", 
                                                                            newUser, descParams);

                    var embed = _embedService.CreateBaseEmbed(payload);
                    await guild
                        .GetTextChannel(channel.ChannelID)
                        .SendEmbedAsync(embed);
                }
            }
            else
            {
                foreach (SocketGuild guild in _client.Guilds)
                {
                    if (!guild.Users.Contains(newUser))
                        continue;

                    var channel = await TryGetLoggingChannel(guild.Id, EventType.UserAvatarUpdated, newUser, guild);
                    if (channel == null)
                        continue;

                    var descParams = new List<string> { user.GetAvatarUrl(), newUser.GetAvatarUrl() };

                    var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.UserUpdated, "user_updated_avatar",
                                                                            newUser, descParams);

                    var embed = _embedService.CreateBaseEmbed(payload);
                    await guild.GetTextChannel(channel.ChannelID).SendEmbedAsync(embed);
                }
            }
        }

        private async Task UserUnbanned(SocketUser user, SocketGuild guild)
        {
            var channel = await TryGetLoggingChannel(guild.Id, EventType.UserUnbanned, user, guild);
            if (channel == null)
                return;

            var descParams = new List<string> { user.FullName(), user.Id.ToString() };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.UserUnbanned, "user_unbanned",
                                                                    user, descParams);

            var embed = _embedService.CreateBaseEmbed(payload);
            await guild.GetTextChannel(channel.ChannelID).SendEmbedAsync(embed);
        }

        private async Task UserBanned(SocketUser user, SocketGuild guild)
        {
            var channel = await TryGetLoggingChannel(guild.Id, EventType.UserBanned, user, guild);
            if (channel == null)
                return;

            var descParams = new List<string> { user.FullName(), user.Id.ToString() };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.UserBanned, "user_banned",
                                                                    user, descParams);

            var embed = _embedService.CreateBaseEmbed(payload);
            await guild.GetTextChannel(channel.ChannelID).SendEmbedAsync(embed);
        }

        private async Task UserLeft(SocketGuildUser user)
        {
            var channel = await TryGetLoggingChannel(user.Guild.Id, EventType.UserLeft, user);
            if (channel == null)
                return;

            var descParams = new List<string> { user.FullName(), user.Id.ToString() };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.UserLeft, "user_left",
                                                                    user, descParams);

            var embed = _embedService.CreateBaseEmbed(payload);
            await user.Guild.GetTextChannel(channel.ChannelID).SendEmbedAsync(embed);
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            var channel = await TryGetLoggingChannel(user.Guild.Id, EventType.UserJoined, user);
            if (channel == null)
                return;

            var descParams = new List<string> { user.FullName(), user.Id.ToString() };

            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Log, EmbedPayloadType.UserJoined, "user_joined",
                                                                    user, descParams);

            var embed = _embedService.CreateBaseEmbed(payload);
            await user.Guild.GetTextChannel(channel.ChannelID).SendEmbedAsync(embed);
        }

        private async Task<ChannelDto> TryGetLoggingChannel(ulong guildID, EventType type, params object[] discordObjects)
        {
            if (discordObjects.Any(o => o == null))
                return null;

            var flagValue = await GetLogConfigFlagValue(guildID);
            if (!IsFlagEnabled(flagValue, type))
                return null;

            var channel = await GetLoggingGuildChannel(guildID);
            return IsChannelDefined(channel) ? channel : null;
        }

        private bool IsUserNull(IUser user) =>
            user == null;

        private bool IsUserBot(IUser user) =>
            user.IsBot;

        private bool IsGuildNull(SocketGuild guild) =>
            guild == null;

        private bool IsMsgNull(IMessage msg) =>
            msg == null;

        private bool IsMsgEmpty(IMessage msg) =>
            msg.Content.Length <= 0;

        private bool IsFlagEnabled(LogConfigDto value, EventType type) =>
            Resolve(value, type);

        private bool Resolve(LogConfigDto logConfig, EventType eventType) =>
            (logConfig.LogFlagValue & (int)eventType) != 0;

        private bool IsChannelDefined(ChannelDto channel) =>
            channel.ChannelID != 0;

        private async Task<LogConfigDto> GetLogConfigFlagValue(ulong guildID)
        {
            var payload = _discordPayloadFactory.CreateDiscordObjectPayload<LogConfigAPIPayload>(new LogConfigDto { GuildID = guildID });
            var result = await _apiService.GetAsync(payload);

            return await result.Content.ReadAsJsonAsync<LogConfigDto>();
        }

        private async Task<ChannelDto> GetLoggingGuildChannel(ulong guildID)
        {
            var payload = _discordPayloadFactory.CreateDiscordObjectPayload<ChannelAPIPayload>(new ChannelDto { GuildID = guildID, ShortName = "log" });
            var result = await _apiService.GetAsync(payload);

            return await result.Content.ReadAsJsonAsync<ChannelDto>();
        }

        private async Task<IEnumerable<GuildDto>> GetAllGuildConfigs()
        {
            var payload = _discordPayloadFactory.CreateDiscordObjectPayload<GuildAPIPayload>(new GuildDto());
            var result = await _apiService.GetAllAsync(payload);

            return await result.Content.ReadAsJsonAsync<IEnumerable<GuildDto>>();
        }
    }
}
