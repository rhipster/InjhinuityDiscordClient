using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class ReactionAPIPayload : IAPIPayload
    {
        private ReactionDto _reaction;
        private const string API_ENDPOINT = "reactions";
        
        public ReactionAPIPayload(ReactionDto reaction)
        {
            _reaction = reaction;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_reaction.GuildID}&name={_reaction.Name}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
            $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            $"{API_ENDPOINT}";

        public string ToJson() =>
            JsonConvert.SerializeObject(_reaction);
    }
}
