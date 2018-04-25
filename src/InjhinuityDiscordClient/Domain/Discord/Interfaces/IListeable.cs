namespace InjhinuityDiscordClient.Domain.Discord.Interfaces
{
    public interface IListable
    {
        string Name { get; set; }
        string ToListString();
    }
}
