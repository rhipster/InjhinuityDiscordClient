using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class MuteRoleAPIPayload : IAPIPayload
    {
        private MuteRoleDto _muteRole;
        private const string API_ENDPOINT = "muterole";

        public MuteRoleAPIPayload(MuteRoleDto muteRole)
        {
            _muteRole = muteRole;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all/?guildID={_muteRole.GuildID}";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_muteRole.GuildID}";

        public string ToPostAPIString() =>
            throw new NotImplementedException();

        public string ToPutAPIString() =>
            $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            throw new NotImplementedException();

        public string ToJson() =>
            JsonConvert.SerializeObject(_muteRole);
    }
}
