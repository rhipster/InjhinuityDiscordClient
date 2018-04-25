using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Interfaces;
using InjhinuityDiscordClient.Services.ModuleServices.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services.ModuleServices
{
    public class DiscordModuleService : IDiscordModuleService
    {
        private readonly IAPIService _apiService;
        private readonly IDiscordPayloadFactory _discordPayloadFactory;

        public DiscordModuleService(IAPIService apiService, IDiscordPayloadFactory discordPayloadFactory)
        {
            _apiService = apiService;
            _discordPayloadFactory = discordPayloadFactory;
        }

        public async Task<HttpResponseMessage> Get<T>(IDiscordObjectDto data) where T : IAPIPayload
        {
            var payload = GetPayload<T>(data);
            return await _apiService.GetAsync(payload);
        }

        public async Task<HttpResponseMessage> Post<T>(IDiscordObjectDto data) where T : IAPIPayload
        {
            var payload = GetPayload<T>(data);
            return await _apiService.PostAsync(payload);
        }

        public async Task<HttpResponseMessage> Put<T>(IDiscordObjectDto data) where T : IAPIPayload
        {
            var payload = GetPayload<T>(data);
            return await _apiService.PutAsync(payload);
        }

        public async Task<HttpResponseMessage> Delete<T>(IDiscordObjectDto data) where T : IAPIPayload
        {
            var payload = GetPayload<T>(data);
            return await _apiService.DeleteAsync(payload);
        }

        public async Task<HttpResponseMessage> GetAll<T>(IDiscordObjectDto data) where T : IAPIPayload
        {
            var payload = GetPayload<T>(data);
            return await _apiService.GetAllAsync(payload);
        }

        private IAPIPayload GetPayload<T>(IDiscordObjectDto data) where T : IAPIPayload =>
            _discordPayloadFactory.CreateDiscordObjectPayload<T>(data);

    }
}
