using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IResources : IService
    {
        string GetResource(string tag);
        string GetFormattedResource(string tag, params string[] args);
        string GetErrorFromCode(string errorCode);
        string GetFormattedErrorFromCode(string errorCode, params string[] args);
        string GetLogEventValue(string logEvent);
    }
}
