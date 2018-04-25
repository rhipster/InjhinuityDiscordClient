using InjhinuityDiscordClient.Domain.Payload.API;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Extensions
{
    public static class HttpExtensions
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string requestUri, IAPIPayload payload)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri) { Content = Serialize(payload) });

        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri, IAPIPayload payload)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = Serialize(payload) });

        public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri, IAPIPayload payload)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = Serialize(payload) });

        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string requestUri, IAPIPayload payload)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(payload) });

        private static HttpContent Serialize(IAPIPayload payload) => new StringContent(payload.ToJson(), Encoding.UTF8, "application/json");
    }
}
