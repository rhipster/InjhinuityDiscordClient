using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using System;

namespace InjhinuityDiscordClient.Dto.Discord
{
    public class EventDto : DiscordObjectDto, IListable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long Date { get; set; }
        public string TimeZone { get; set; }
        public long Duration { get; set; }
        public bool Started { get; set; }

        public string ToListString() =>
            $"{Name} | {DateTimeOffset.FromUnixTimeMilliseconds(Date)} | {TimeZone} | {Duration / 1000 * 60}";
    }
}
