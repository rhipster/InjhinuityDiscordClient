using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class RoleAPIPayload : IAPIPayload
    {
        private RoleDto _role;
        private const string API_ENDPOINT = "roles";
        
        public RoleAPIPayload(RoleDto role)
        {
            _role = role;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all?guildID={_role.GuildID}";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_role.GuildID}&name={_role.Name}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
            throw new NotImplementedException();

        public string ToDeleteAPIString() =>
            $"{API_ENDPOINT}";

        public string ToJson() =>
            JsonConvert.SerializeObject(_role);
    }
}
