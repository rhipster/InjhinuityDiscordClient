using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.ModuleServices.Interfaces
{
    public interface IParamService : IService
    {
        string GetParamName(string shortName);
    }
}
