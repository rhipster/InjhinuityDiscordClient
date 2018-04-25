using Discord;
using InjhinuityDiscordClient.Services.Injection;
using System.Collections.Immutable;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IBotCredentials : IService
    {
        ulong ClientID { get; }
        string Token { get; }
        string ApiClientID { get; }
        string ApiClientSecret { get; }
        ImmutableArray<ulong> OwnerIDs { get; }

        bool IsOwner(IUser u);
    }
}
