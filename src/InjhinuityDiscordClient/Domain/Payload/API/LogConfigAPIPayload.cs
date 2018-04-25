using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class LogConfigAPIPayload : IAPIPayload
    {
        private LogConfigDto _logConfig;
        private const string API_ENDPOINT = "logconfig";
        
        public LogConfigAPIPayload(LogConfigDto logConfig)
        {
            _logConfig = logConfig;
        }

        public string ToGetAllAPIString() =>
            throw new NotImplementedException();

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_logConfig.GuildID}";

        public string ToPostAPIString() =>
            throw new NotImplementedException();

        public string ToPutAPIString() =>
            $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            throw new NotImplementedException();

        public string ToJson() =>
            JsonConvert.SerializeObject(_logConfig);
    }
}
