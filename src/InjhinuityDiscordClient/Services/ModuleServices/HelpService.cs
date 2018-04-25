using Discord;
using Discord.Commands;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InjhinuityDiscordClient.Services.ModuleServices
{
    public class HelpService : IHelpService
    {
        private readonly CommandService _commandService;

        public HelpService(CommandService commandService)
        {
            _commandService = commandService;
        }

        public List<(string, string, bool)> GetCommandInfos(GuildPermissions perms)
        {
            var commandInfoList = new List<(string, string, bool)>();

            //TODO: Inline option
            foreach (var commandInfo in _commandService.Commands)
            {
                var permPrecondition = (RequireUserPermissionAttribute) commandInfo.Preconditions.FirstOrDefault(x => x is RequireUserPermissionAttribute);

                if (permPrecondition == null)
                    commandInfoList.Add((commandInfo.Name, commandInfo.Summary, false));
                else if (perms.BanMembers && permPrecondition.GuildPermission == GuildPermission.BanMembers)
                    commandInfoList.Add((commandInfo.Name, commandInfo.Summary, false));
            }

            return commandInfoList
                .OrderBy(x => x.Item1)
                .ToList();
        }
    }
}
