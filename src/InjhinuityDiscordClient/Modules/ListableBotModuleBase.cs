using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Enums;
using System;

namespace InjhinuityDiscordClient.Modules
{
    public abstract class ListableBotModuleBase<T, K> : APIBotModuleBase<T> 
                                               where T : IAPIPayload
                                               where K : IListable
    {
        public ListableBotModuleBase(IBotConfig botConfig, IDiscordModuleService discordModuleService, IEmbedService embedService, 
                                     IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, discordModuleService, embedService, embedPayloadFactory, resources)
        {
        }

        protected async Task<HttpResponseMessage> GetAll(IDiscordObjectDto data) =>
            await _discordModuleService.GetAll<T>(data);

        protected async void CreateAndSendList(HttpContent content)
        {
            var list = await content.ReadAsJsonAsync<List<K>>();
            if (list == null)
                return;

            var objName = typeof(K).Name;

            var descString = "";
            list.ForEach(x => descString += $"\n{((IListable)x).ToListString()}");

            var descParams = new List<string> { _botConfig.Prefix, descString };
            var titleParams = new List<string> { FormatObjectName(objName), Context.Guild.Name };

            var embedPayload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.List, GetEmbedTypeFromObjectType(objName), 
                                                                        FormatObjectName(objName).ToLower(), Context.Message.Author,
                                                                        descParams, titleParams);

            var embed = _embedService.CreateBaseEmbed(embedPayload);
            await SendEmbedAsync(embed);
        }

        // TODO: Find some better way of doing this
        private EmbedPayloadType GetEmbedTypeFromObjectType(string objName)
        {
            switch(objName)
            {
                case "ChannelDto":
                    return EmbedPayloadType.Channel;
                case "CommandDto":
                    return EmbedPayloadType.Command;
                case "EventDto":
                    return EmbedPayloadType.Event;
                case "ReactionDto":
                    return EmbedPayloadType.Reaction;
                case "RoleDto":
                    return EmbedPayloadType.Role;
                case "ParamDto":
                    return EmbedPayloadType.Param;
                default:
                    throw new Exception($"Couldn't convert listable type {objName} to an EmbedType.");
            }
        }

        private string FormatObjectName(string name) =>
            name.Replace("Dto", "");
    }
}
