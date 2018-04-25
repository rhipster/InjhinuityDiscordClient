using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Services.Injection;
using System.Net.Http;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IAPIService : IService
    {
        Task<HttpResponseMessage> GetAllAsync(IAPIPayload payload);
        Task<HttpResponseMessage> GetAsync(IAPIPayload payload);
        Task<HttpResponseMessage> PostAsync(IAPIPayload payload);
        Task<HttpResponseMessage> PutAsync(IAPIPayload payload);
        Task<HttpResponseMessage> DeleteAsync(IAPIPayload payload);
    }
}
