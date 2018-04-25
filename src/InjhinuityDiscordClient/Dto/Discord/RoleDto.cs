using InjhinuityDiscordClient.Domain.Discord.Interfaces;

namespace InjhinuityDiscordClient.Dto.Discord
{
    public class RoleDto : DiscordObjectDto, IListable
    {
        public string Name { get; set; }
        public ulong RoleID { get; set; }

        public string ToListString() =>
            $"{Name}";
    }
}
