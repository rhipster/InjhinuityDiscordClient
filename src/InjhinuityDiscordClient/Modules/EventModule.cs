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
    public class EventModule : ListableBotModuleBase<EventAPIPayload, EventDto>
    {
        private readonly IFormattingService _formattingService;

        public EventModule(IDiscordModuleService discordModuleService, IBotConfig botConfig, IEmbedService embedService,
                           IEmbedPayloadFactory embedPayloadFactory, IResources resources, IFormattingService formattingService)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
            _formattingService = formattingService;
        }

        //TODO: Convert these to embed conversations soon tm (Removes string date/duration)
        [Command("ev create"), Summary("Creates an event. [event_name] [event_desc] [event_date] [event_timezone] [event_duration]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Create(string name, string description, string date, string timeZone, string duration)
        {
            if (!FormatDate(date, timeZone, out var unixDate))
            {
                await HandleCommandResult("date", false);
                return;
            }

            if (!FormatDuration(date, out var unixDuration))
            {
                await HandleCommandResult("duration", false);
                return;
            }

            //TODO: Check validations (maybe redo)

            await Post("post", GetDto(Context.Guild.Id, name, description, long.Parse(date), timeZone, long.Parse(duration)));
        }

        [Command("ev update"), Summary("Updates an event. [event_name] [event_desc] [event_date] [event_timezone] [event_duration]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Update(string name, string description, string date, string timeZone, string duration)
        {
            if (!FormatDate(date, timeZone, out var unixDate))
            {
                await HandleCommandResult("date", false);
                return;
            }

            if (!FormatDuration(date, out var unixDuration))
            {
                await HandleCommandResult("duration", false);
                return;
            }

            //TODO: Check validations (maybe redo)

            await Put("put", GetDto(Context.Guild.Id, name, description, long.Parse(date), timeZone, long.Parse(duration)));
        }

        [Command("ev delete"), Summary("Deletes an event. [event_name] [event_date]"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Delete(string name, string date)
        {
            await Delete("delete", GetDto(Context.Guild.Id, name));
        }

        [Command("ev list"), Summary("Lists every event available for the current guild.")]
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

        private bool FormatDate(string date, string timeZone, out long unix)
        {
            if (_formattingService.TryFormatDateToUnix(date, timeZone, out var result))
                unix = result;
            else
                unix = 0;

            return unix != 0;
        }

        private bool FormatDuration(string duration, out long unix)
        {
            if (_formattingService.TryFormatDurationToUnix(duration, out var result))
                unix = result;
            else
                unix = 0;

            return unix != 0;
        }

        private IDiscordObjectDto GetDto(ulong guildID, string name = "", string description = "", long date = 0, string timeZone = "", long duration = 0, bool started = false) =>
            new EventDto { GuildID = guildID, Name = name, Description = description, Date = date, TimeZone = timeZone, Duration = duration, Started = started };
    }
}
