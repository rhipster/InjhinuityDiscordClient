using Discord;
using InjhinuityDiscordClient.Services.Injection;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Services.ModuleServices.Interfaces
{
    public interface IHelpService : IService
    {
        List<(string, string, bool)> GetCommandInfos(GuildPermissions perms);
    }
}
