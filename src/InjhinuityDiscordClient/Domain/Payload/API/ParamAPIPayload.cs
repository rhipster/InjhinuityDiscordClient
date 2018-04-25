using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class ParamAPIPayload : IAPIPayload
    {
        private ParamDto _param;
        private const string API_ENDPOINT = "params";
        
        public ParamAPIPayload(ParamDto param)
        {
            _param = param;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all?guildID={_param.GuildID}";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_param.GuildID}&shortname={_param.ShortName}";

        public string ToPostAPIString() =>
            throw new NotImplementedException();

        public string ToPutAPIString() =>
            $"{API_ENDPOINT}";
        
        public string ToDeleteAPIString() =>
            throw new NotImplementedException();

        public string ToJson() =>
            JsonConvert.SerializeObject(_param);
    }
}
