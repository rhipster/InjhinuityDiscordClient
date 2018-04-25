using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Services.Implementation
{
    public class Resources : IResources
    {
        private Dictionary<string, string> _resources;
        private Dictionary<string, string> _errors;
        private Dictionary<string, string> _logEvents;

        public Resources(IFileReader fileReader, IBotConfig botConfig)
        {
            var json = fileReader.ReadFile(botConfig.ResourcesPath);
            _resources = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            json = fileReader.ReadFile(botConfig.ErrorsPath);
            _errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            json = fileReader.ReadFile(botConfig.LogEventsPath);
            _logEvents = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public string GetResource(string tag)
        {
            var res = _resources.GetValueOrDefault(tag);
            return res ?? throw new Exception($"Missing resource tag: {tag}");
        }

        public string GetErrorFromCode(string errorCode)
        {
            var res = _errors.GetValueOrDefault(errorCode);
            return res ?? throw new Exception($"Missing error code: {errorCode}");
        }

        public string GetLogEventValue(string logEvent)
        {
            var res = _logEvents.GetValueOrDefault(logEvent);
            return res ?? throw new Exception($"Missing log event name: {logEvent}");
        }

        public string GetFormattedResource(string tag, params string[] args) =>
            string.Format(GetResource(tag), args);

        public string GetFormattedErrorFromCode(string errorCode, params string[] args) =>
            string.Format(GetErrorFromCode(errorCode), args);
    }
}
