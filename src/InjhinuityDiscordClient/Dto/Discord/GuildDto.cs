using System.Collections.Generic;

namespace InjhinuityDiscordClient.Dto.Discord
{
    public class GuildDto : DiscordObjectDto
    {
        public ICollection<CommandDto> GuildCommands { get; set; } = new HashSet<CommandDto>();
        public ICollection<EventDto> GuildEvents { get; set; } = new HashSet<EventDto>();
        public ICollection<ParamDto> GuildParams { get; set; } = new HashSet<ParamDto>();
        public ICollection<ChannelDto> GuildChannels { get; set; } = new HashSet<ChannelDto>();
        public ICollection<UserMessageDto> UserMessages { get; set; } = new HashSet<UserMessageDto>();
        public ICollection<ReactionDto> GuildReactions { get; set; } = new HashSet<ReactionDto>();
        public ICollection<RoleDto> GuildRoles { get; set; } = new HashSet<RoleDto>();

        //public ICollection<CommandCooldown> CommandCooldowns { get; set; } = new HashSet<CommandCooldown>();

        public LogConfigDto LogConfig { get; set; } = new LogConfigDto();
        public MuteRoleDto MuteRole { get; set; } = new MuteRoleDto();
        public EventRoleDto EventRole { get; set; } = new EventRoleDto();
    }
}
