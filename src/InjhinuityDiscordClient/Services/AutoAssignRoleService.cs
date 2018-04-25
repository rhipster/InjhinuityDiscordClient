using Discord.WebSocket;
using InjhinuityDiscordClient.Dto.Discord;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Services
{
    public class AutoAssignRoleService : IAutoAssignRoleService
    {
        private readonly IAPIService _apiService;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;
        private readonly IParamService _paramService;

        public AutoAssignRoleService(IAPIService apiService, IDiscordPayloadFactory discordPayloadFactory, DiscordSocketClient client,
                                     IParamService paramService)
        {
            _apiService = apiService;
            _discordPayloadFactory = discordPayloadFactory;
            _paramService = paramService;

            client.UserJoined += UserJoinedGuild;
        }

        private async Task UserJoinedGuild(SocketGuildUser arg)
        {
            var paramAAR = new ParamDto { GuildID = arg.Guild.Id, ShortName = "aar" };
            var paramAARPayload = _discordPayloadFactory.CreateDiscordObjectPayload<ParamAPIPayload>(paramAAR);

            var resultAAR = await _apiService.GetAsync(paramAARPayload);

            if (!resultAAR.IsSuccessStatusCode)
                return;

            var paramAARID = new ParamDto { GuildID = arg.Guild.Id, ShortName = "aarid" };
            var paramAARIDPayload = _discordPayloadFactory.CreateDiscordObjectPayload<ParamAPIPayload>(paramAARID);

            var resultAARID = await _apiService.GetAsync(paramAARIDPayload);

            if (!resultAARID.IsSuccessStatusCode)
                return;

            var paramReturn = JsonConvert.DeserializeObject<ParamDto>(await resultAARID.Content.ReadAsStringAsync());
            if (!ulong.TryParse(paramReturn.Value, out var value))
                return;

            var role = arg.Guild.Roles.FirstOrDefault(x => x.Id == value);
            if (role != null)
                await arg.AddRoleAsync(role);
        }
    }
}
