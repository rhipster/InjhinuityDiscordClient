using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IFileReader : IService
    {
        string ReadFile(string path);
    }
}
