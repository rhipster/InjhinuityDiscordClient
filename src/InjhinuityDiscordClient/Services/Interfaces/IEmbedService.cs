using Discord;
using InjhinuityDiscordClient.Domain.Payload.Embed.Interfaces;
using InjhinuityDiscordClient.Services.Injection;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IEmbedService : IService
    {
        EmbedBuilder CreateBaseEmbed(IEmbedPayload payload);
        EmbedBuilder CreateFieldEmbed(IEmbedPayload payload);
    }
}
