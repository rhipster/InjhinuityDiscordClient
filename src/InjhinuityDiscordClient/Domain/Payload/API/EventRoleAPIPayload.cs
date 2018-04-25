using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class EventRoleAPIPayload : IAPIPayload
    {
        private EventRoleDto _eventRole;
        private const string API_ENDPOINT = "eventrole";

        public EventRoleAPIPayload(EventRoleDto eventRole)
        {
            _eventRole = eventRole;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_eventRole.GuildID}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
           $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            $"{API_ENDPOINT}";

        public string ToJson() =>
            JsonConvert.SerializeObject(_eventRole);
    }
}
