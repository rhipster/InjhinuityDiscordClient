using System.Collections.Generic;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using Newtonsoft.Json;

namespace InjhinuityDiscordClient.Services.ModuleServices
{
    public class ParamService : IParamService
    {
        private readonly IFileReader _fileReader;
        private readonly Dictionary<string, string> _params;

        public ParamService(IFileReader fileReader, IBotConfig botConfig)
        {
            _fileReader = fileReader;

            var json = fileReader.ReadFile(botConfig.GuildParamsPath);
            _params = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public string GetParamName(string shortName)
        {
            var name = _params.GetValueOrDefault(shortName);
            return name != "" ? name : false.ToString(); 
        }

    }
}
