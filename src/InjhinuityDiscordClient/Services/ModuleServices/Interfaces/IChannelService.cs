using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.ModuleServices.Interfaces
{
    public interface IChannelService : IService
    {
        string GetChannelName(string shortName);
    }
}
