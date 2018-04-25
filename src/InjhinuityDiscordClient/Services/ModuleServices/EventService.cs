using Discord.WebSocket;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace InjhinuityDiscordClient.Services.ModuleServices
{
    public class EventService : IEventService
    {
        private long upcomingDelayShort = 10 * 60 * 1000; //10 minutes
        private long upcomingDelayLong = 60 * 60 * 1000; //60 minutes
        private readonly Timer _timer;
        private readonly IAPIService _apiService;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;
        private readonly IDiscordModuleService _discordModuleService;
        private readonly IEmbedPayloadFactory _embedPayloadFactory;
        private readonly IEmbedService _embedService;
        private readonly IOwnerLogger _ownerLogger;
        private readonly IResources _resouces;
        private readonly DiscordSocketClient _client;

        public EventService(IAPIService apiService, IDiscordModuleService discordModuleService, IOwnerLogger ownerLogger, 
                            DiscordSocketClient client, IResources resources, IEmbedPayloadFactory embedPayloadFactory, 
                            IEmbedService embedService, IDiscordPayloadFactory discordPayloadFactory)
        {
            _apiService = apiService;
            _discordModuleService = discordModuleService;
            _embedPayloadFactory = embedPayloadFactory;
            _embedService = embedService;
            _ownerLogger = ownerLogger;
            _resouces = resources;

            _timer = new Timer(1 * 60 * 1000); //10 minutes
            _timer.Elapsed += new ElapsedEventHandler(OnTimeElapsed);
            _timer.Start();
        }

        private async void OnTimeElapsed(object source, ElapsedEventArgs e)
        {
            foreach (var guild in _client.Guilds) {
                var channel = await TryGettingChannel(guild);
                if (channel == null)
                {
                    await _ownerLogger.LogError("0007", "", guild.Name);
                    return;
                }

                var events = await TryGettingEvents(guild);
                if (events == null)
                {
                    await _ownerLogger.LogError("0006", "", guild.Name);
                    return;
                }

                var fields = new List<(string, string, bool)>();

                FillEventsField("started_event_field_title", fields, FindStartedEvents(events));
                FillEventsField("upcoming_short_event_field_title", fields, FindUpcomingEventsShort(events));
                FillEventsField("upcoming_long_field_title", fields, FindUpcomingEventsLong(events));

                string msg = "";
                if (FindStartedEvents(events).Any())
                {
                    var eventRole = await TryGettingEventRole(guild);
                    var guildRole = guild.GetRole(eventRole.RoleID);

                    if (guildRole != null)
                        msg = guildRole.Mention;
                }

                var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.Success, EmbedPayloadType.Event, "event",
                                                                        null, null, null, null, fields);

                var embed = _embedService.CreateFieldEmbed(payload);
                await guild
                    .GetTextChannel(channel.ChannelID)
                    .SendEmbedMsgAsync(msg, embed);
            }
        }

        private async Task<ChannelDto> TryGettingChannel(SocketGuild guild)
        {
            var channelResult = await _discordModuleService.GetAll<ChannelAPIPayload>(new ChannelDto { GuildID = guild.Id, ShortName = "event" });
            
            return channelResult.IsSuccessStatusCode ?
                await channelResult.Content.ReadAsJsonAsync<ChannelDto>() :
                null;
        }

        private async Task<List<EventDto>> TryGettingEvents(SocketGuild guild)
        {
            var eventsResult = await _discordModuleService.GetAll<EventAPIPayload>(new EventDto { GuildID = guild.Id });

            return eventsResult.IsSuccessStatusCode ?
                await eventsResult.Content.ReadAsJsonAsync<List<EventDto>>() :
                null;
        }

        private async Task<EventRoleDto> TryGettingEventRole(SocketGuild guild)
        {
            var roleResult = await _discordModuleService.Get<EventRoleAPIPayload>(new EventRoleDto { GuildID = guild.Id });

            return roleResult.IsSuccessStatusCode ? 
                await roleResult.Content.ReadAsJsonAsync<EventRoleDto>() : 
                null;
        }

        private void FillEventsField(string titleResx, List<(string, string, bool)> fields, List<EventDto> events)
        {
            if (!events.Any())
                return;

            var fieldTitle = _resouces.GetResource(titleResx);
            var fieldDesc = "";
            events.ForEach(x => fieldDesc += $"{x.ToString()}\n");

            fields.Add((fieldTitle, fieldDesc, false));
        }

        private List<EventDto> FindStartedEvents(IEnumerable<EventDto> events) =>
            events.Where(x => x.Started).ToList();

        private List<EventDto> FindUpcomingEventsShort(IEnumerable<EventDto> events) =>
            events.Where(x => 
                (x.Date - upcomingDelayShort < DateTimeOffset.Now.Ticks) && !x.Started
            ).ToList();

        private List<EventDto> FindUpcomingEventsLong(IEnumerable<EventDto> events) =>
            events.Where(x =>
                (x.Date - upcomingDelayLong < DateTimeOffset.Now.Ticks) && !x.Started
            ).ToList();
    }
}
