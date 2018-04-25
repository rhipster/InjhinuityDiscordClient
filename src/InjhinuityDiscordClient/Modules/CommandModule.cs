using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Modules
{
    public class CommandModule : ListableBotModuleBase<CommandAPIPayload, CommandDto>
    {
        public CommandModule(IDiscordModuleService discordModuleService, IBotConfig botConfig, IEmbedService embedService,
                             IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        [Command("cm create"), Summary("Creates a custom command. [command_name] [command_body]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Create(string name, [Remainder]string body)
        {
            await Post("post", GetDto(Context.Guild.Id, name, body));
        }

        [Command("cm update"), Summary("Updates a custom command. [command_name] [command_body]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Update(string name, [Remainder]string body)
        {
            await Put("put", GetDto(Context.Guild.Id, name, body));
        }

        [Command("cm delete"), Summary("Deletes a custom command. [command_name]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Delete(string name)
        {
            await Delete("delete", GetDto(Context.Guild.Id, name));
        }

        [Command("cm list"), Summary("Lists every command available for the current guild.")]
        public async Task List()
        {
            var result = await GetAll(GetDto(Context.Guild.Id));

            if (result.IsSuccessStatusCode)
            {
                CreateAndSendList(result.Content);
                return;
            }

            await HandleAPIResult(result, "get");
        }

        private IDiscordObjectDto GetDto(ulong guildID, string name = "", string body = "") =>
            new CommandDto { GuildID = guildID, Name = name, Body = body };
    }
}
