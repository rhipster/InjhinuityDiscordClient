namespace InjhinuityDiscordClient.Dto.Discord
{
    public class UserMessageDto : DiscordObjectDto
    {
        public ulong MessageID { get; set; }
        public ulong AuthorID { get; set; }
        public string Content { get; set; }
    }
}
