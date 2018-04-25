using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Services.Injection;
using System.Net.Http;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services.ModuleServices.Interfaces
{
    public interface IDiscordModuleService : IService
    {
        Task<HttpResponseMessage> Get<T>(IDiscordObjectDto data) where T : IAPIPayload;
        Task<HttpResponseMessage> Post<T>(IDiscordObjectDto data) where T : IAPIPayload;
        Task<HttpResponseMessage> Put<T>(IDiscordObjectDto data) where T : IAPIPayload;
        Task<HttpResponseMessage> Delete<T>(IDiscordObjectDto data) where T : IAPIPayload;
        Task<HttpResponseMessage> GetAll<T>(IDiscordObjectDto data) where T : IAPIPayload;
    }
}
