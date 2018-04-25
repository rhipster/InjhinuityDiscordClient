using InjhinuityDiscordClient.Domain.Discord.Interfaces;

namespace InjhinuityDiscordClient.Dto.Discord
{
    public class ParamDto : DiscordObjectDto, IListable
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string ToListString() =>
            $"{Name}: {Value}";
    }
}
