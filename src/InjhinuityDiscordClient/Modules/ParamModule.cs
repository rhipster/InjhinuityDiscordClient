using Discord;
using Discord.Commands;
using InjhinuityDiscordClient.Dto.Discord;
using System.Threading.Tasks;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Modules
{
    public class ParamModule : ListableBotModuleBase<ParamAPIPayload, ParamDto>
    {
        private readonly IParamService _paramService;

        public ParamModule(IParamService paramService, IDiscordModuleService discordModuleService, IBotConfig botConfig, 
                           IEmbedService embedService, IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
            _paramService = paramService;
        }

        [Command("adg"), Alias("adi", "aar", "sgm", "ualee", "le"), Summary("Toggles a given parameter in the guild's config."), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BooleanParam()
        {
            await Put("toggle", GetDto(Context.Guild.Id, GetShortName()));
        }

        [Command("aarid"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task AutoAssignRoleID([Remainder]IRole role = null)
        {
            var action = role == null ? "reset" : "put";
            var id = role?.Id.ToString() ?? "0";
            await Put(action, GetDto(Context.Guild.Id, GetShortName(), id));
        }

        [Command("gm"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task GreetMessage([Remainder]string greet = "")
        {
            await Put("put", GetDto(Context.Guild.Id, GetShortName(), greet));
        }

        [Command("pm list"), RequireUserPermission(GuildPermission.BanMembers)]
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

        private string GetShortName() =>
            (Context.Message.Content.Split(' ')[0]).Substring(1);

        private IDiscordObjectDto GetDto(ulong guildID, string shortName = "", string value = "") =>
            new ParamDto { GuildID = guildID, ShortName = shortName, Value = value };
    }
}
