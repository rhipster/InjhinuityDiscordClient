using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IColorConverter : IService
    {
        string GetColorCodeFromPayloadType(EmbedPayloadType type);
    }
}
