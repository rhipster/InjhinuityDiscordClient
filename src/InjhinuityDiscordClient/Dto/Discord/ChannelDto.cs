using InjhinuityDiscordClient.Domain.Discord.Interfaces;

namespace InjhinuityDiscordClient.Dto.Discord
{
    public class ChannelDto : DiscordObjectDto, IListable
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public ulong ChannelID { get; set; }

        public string ToListString() =>
            $"{Name}: {ChannelID}";
    }
}
