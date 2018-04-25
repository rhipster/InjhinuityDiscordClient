using InjhinuityDiscordClient.Domain.Discord.Interfaces;

namespace InjhinuityDiscordClient.Dto.Discord
{
    public class CommandDto : DiscordObjectDto, IListable
    {
        public string Name { get; set; }
        public string Body { get; set; }

        public string ToListString() =>
            $"{Name}";
    }
}
