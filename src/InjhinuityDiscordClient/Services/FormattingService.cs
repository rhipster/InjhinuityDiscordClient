using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace InjhinuityDiscordClient.Services
{
    public class FormattingService : IFormattingService
    {
        private const string DATE_FORMAT = "mm/dd/yyyy hh:mm:ss";
        private const string DURATION_FORMAT = "hh:mm";
        private const long UNIX_IN_MILLIS_CONST = 60 * 1000;
        private readonly Dictionary<string, int> _timezones;

        public FormattingService(IFileReader fileReader, IBotConfig botConfig)
        {
            var json = fileReader.ReadFile(botConfig.RegionsPath);
            _timezones = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }

        public string ExtractIDFromMention(string mention) =>
            mention.Trim(new char[] { '<', '>', '@', '!' });

        public bool TryFormatDateToUnix(string date, string timezone, out long unix)
        {
            if (DateTimeOffset.TryParseExact(date, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
            {
                if (_timezones.TryGetValue(timezone, out var timezoneOffset))
                    unix = result.ToUnixTimeMilliseconds() - (timezoneOffset * UNIX_IN_MILLIS_CONST);
                else
                    unix = 0;
            }
            else
            {
                unix = 0;
            }

            return unix != 0;
        }

        public bool TryFormatDurationToUnix(string duration, out long unix)
        {
            if (DateTimeOffset.TryParseExact(duration, DURATION_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
                unix = result.ToUnixTimeMilliseconds();
            else
                unix = 0;

            return unix != 0;
        }
    }
}
