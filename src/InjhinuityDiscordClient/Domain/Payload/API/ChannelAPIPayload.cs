using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;
using System;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class ChannelAPIPayload : IAPIPayload
    {
        private ChannelDto _channel;
        private const string API_ENDPOINT = "channels";

        public ChannelAPIPayload(ChannelDto channel)
        {
            _channel = channel;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all?guildID={_channel.GuildID}";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_channel.GuildID}&shortName={_channel.ShortName}";

        public string ToPostAPIString() =>
            throw new NotImplementedException();

        public string ToPutAPIString() =>
            $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            throw new NotImplementedException();

        public string ToJson() =>
            JsonConvert.SerializeObject(_channel);
    }
}
