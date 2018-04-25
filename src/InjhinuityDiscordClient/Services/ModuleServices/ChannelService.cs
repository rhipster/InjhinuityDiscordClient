using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Services.ModuleServices
{
    public class ChannelService : IChannelService
    {
        private readonly IFileReader _fileReader;
        private readonly Dictionary<string, string> _channels;

        public ChannelService(IFileReader fileReader, IBotConfig botConfig)
        {
            _fileReader = fileReader;

            var json = _fileReader.ReadFile(botConfig.ChannelIDSPath);
            _channels = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public string GetChannelName(string shortName) =>
            _channels.GetValueOrDefault(shortName);
    }
}
