using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Modules
{
    public abstract class APIBotModuleBase<T> : BotModuleBase where T : IAPIPayload
    {
        private readonly Dictionary<string, string> _apiActions = new Dictionary<string, string>();
        protected readonly IDiscordModuleService _discordModuleService;

        public APIBotModuleBase(IBotConfig botConfig, IDiscordModuleService discordModuleService, IEmbedService embedService, 
                                IEmbedPayloadFactory embedPayloadFactory, IResources resources)
            : base(botConfig, embedService, embedPayloadFactory, resources)
        {
            _discordModuleService = discordModuleService;
        }

        protected async Task<HttpResponseMessage> Get(IDiscordObjectDto data) => 
            await _discordModuleService.Get<T>(data);

        protected async Task Post(string action, IDiscordObjectDto data)
        {
            var result = await _discordModuleService.Post<T>(data);
            await HandleAPIResult(result, action);
        }

        protected async Task Put(string action, IDiscordObjectDto data)
        {
            var result = await _discordModuleService.Put<T>(data);
            await HandleAPIResult(result, action);
        }

        protected async Task Delete(string action, IDiscordObjectDto data)
        {
            var result = await _discordModuleService.Delete<T>(data);
            await HandleAPIResult(result, action);
        }

        protected async Task HandleAPIResult(HttpResponseMessage result, string action)
        {
            var embedStruct = GetStruct(result.IsSuccessStatusCode);
            var embedType = GetType(result.IsSuccessStatusCode);

            var descParams = new List<string> { FormatAPIObjectName(typeof(T)) };
            var footerParams = new List<string> { result.StatusCode.ToString() };

            var payload = _embedPayloadFactory.CreateEmbedPayload(embedStruct, embedType, action,
                                                                    Context.User, descParams, null, footerParams);

            var embed = _embedService.CreateBaseEmbed(payload);
            await SendDeletableEmbed(embed);
        }

        //TODO: Figure out a way around this
        private string FormatAPIObjectName(Type type) =>
            type.Name.Split("API")[0];
    }
}
