using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class GuildAPIPayload : IAPIPayload
    {
        private GuildDto _guild;
        private const string API_ENDPOINT = "guilds";
        
        public GuildAPIPayload(GuildDto guild)
        {
            _guild = guild;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_guild.GuildID}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
            throw new NotImplementedException();

        public string ToDeleteAPIString() =>
            throw new NotImplementedException();

        public string ToJson() =>
            JsonConvert.SerializeObject(_guild);
    }
}
