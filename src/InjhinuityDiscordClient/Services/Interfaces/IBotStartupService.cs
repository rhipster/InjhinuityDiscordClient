using InjhinuityDiscordClient.Services.Injection;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IBotStartupService : IService
    {
        Task SynchroniseGuilds();
    }
}
