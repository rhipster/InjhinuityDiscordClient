using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class EventAPIPayload : IAPIPayload
    {
        private EventDto _event;
        private const string API_ENDPOINT = "events";
        
        public EventAPIPayload(EventDto @event)
        {
            _event = @event;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_event.GuildID}&name={_event.Name}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
           $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            $"{API_ENDPOINT}";

        public string ToJson() =>
            JsonConvert.SerializeObject(_event);
    }
}
