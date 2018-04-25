using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IFormattingService : IService
    {
        string ExtractIDFromMention(string mention);
        bool TryFormatDateToUnix(string date, string timezone, out long unix);
        bool TryFormatDurationToUnix(string duration, out long unix);
    }
}
