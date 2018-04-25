using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Modules
{
    public class ChannelModule : ListableBotModuleBase<ChannelAPIPayload, ChannelDto>
    {
        private readonly IChannelService _channelService;

        public ChannelModule(IDiscordModuleService discordModuleService, IChannelService channelService, IBotConfig botConfig, 
                             IEmbedService embedService, IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
            _channelService = channelService;
        }

        [Command("ch update"), Summary("Updates a given channel with the ID of the channel the command is used in."), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Update(string shortName)
        {
            var name = _channelService.GetChannelName(shortName);
            await Put("put", GetDto(Context.Guild.Id, name, shortName, Context.Channel.Id));
        }

        [Command("ch reset"), Summary("Resets a given channel ID."), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Reset(string shortName)
        {
            var name = _channelService.GetChannelName(shortName);
            await Put("put", GetDto(Context.Guild.Id, name, shortName));
        }

        [Command("ch list"), Summary("Lists every channel configuration for the current guild."), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task List()
        {
            var result = await GetAll(GetDto(Context.Guild.Id));

            if (result.IsSuccessStatusCode)
            {
                CreateAndSendList(result.Content);
                return;
            }

            await HandleAPIResult(result, "get");
        }

        private IDiscordObjectDto GetDto(ulong guildID, string name = "", string shortName = "", ulong channelID = 0) =>
            new ChannelDto { GuildID = guildID, Name = name, ShortName = shortName, ChannelID = channelID };
    }
}
