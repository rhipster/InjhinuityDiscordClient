using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class UserMessageAPIPayload : IAPIPayload
    {
        private UserMessageDto _userMessage;
        private const string API_ENDPOINT = "usermessages";
        
        public UserMessageAPIPayload(UserMessageDto userMessage)
        {
            _userMessage = userMessage;
        }

        public string ToGetAllAPIString() =>
            throw new NotImplementedException();

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_userMessage.GuildID}&messageID={_userMessage.MessageID}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
            throw new NotImplementedException();

        public string ToDeleteAPIString() =>
            throw new NotImplementedException();

        public string ToJson() =>
            JsonConvert.SerializeObject(_userMessage);
    }
}
