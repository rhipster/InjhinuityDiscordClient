using InjhinuityDiscordClient.Dto.Discord;
using Newtonsoft.Json;

namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public class CommandAPIPayload : IAPIPayload
    {
        private CommandDto _command;
        private const string API_ENDPOINT = "commands";
        
        public CommandAPIPayload(CommandDto command)
        {
            _command = command;
        }

        public string ToGetAllAPIString() =>
            $"{API_ENDPOINT}/all?guildID={_command.GuildID}";

        public string ToGetAPIString() =>
            $"{API_ENDPOINT}?guildID={_command.GuildID}&name={_command.Name}";

        public string ToPostAPIString() =>
            $"{API_ENDPOINT}";

        public string ToPutAPIString() =>
            $"{API_ENDPOINT}";

        public string ToDeleteAPIString() =>
            $"{API_ENDPOINT}";

        public string ToJson() =>
            JsonConvert.SerializeObject(_command);
    }
}
