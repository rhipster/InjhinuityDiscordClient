using IdentityModel.Client;
using InjhinuityDiscordClient.Domain.Exceptions;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Services.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InjhinuityDiscordClient.Extensions;

namespace InjhinuityDiscordClient.Services
{
    public class APIService : IAPIService
    {
        private const string JHINBOT_API = "InjhinuityApi";
        private const string URI = "http://localhost:4001/api/";
        private const string IDENTITY_URI = "http://localhost:4000";
        private const string CLIENT_ENDPOINT = "discord/";
        private readonly IBotCredentials _botCredentials;
        private long _tokenExpiresTime;
        private TokenResponse _token;

        private bool IsTokenExpired => 
            DateTimeOffset.Now.ToUnixTimeSeconds() >= _tokenExpiresTime;

        public APIService(IBotCredentials botCredentials)
        {
            _botCredentials = botCredentials;
            _token = null;
        }

        /// <summary>
        /// Standard Get call to our api which will fetch every item of a certain collection
        /// </summary>
        /// <param name="payload">Contains all our data to send or modify</param>
        /// <returns>Returns the response from the API containing our result code and extra display data.</returns>
        public async Task<HttpResponseMessage> GetAllAsync(IAPIPayload payload)
        {
            using (var client = new HttpClient())
            {
                await RefreshTokenIfExpired();

                client.SetBearerToken(_token.AccessToken);
                var response = await client.GetAsync($"{URI}{CLIENT_ENDPOINT}{payload.ToGetAllAPIString()}");

                return response;
            }
        }

        /// <summary>
        /// Standard Get call to our api using a specific payload containing an object to
        /// use inside our endpoint.
        /// </summary>
        /// <param name="payload">Contains all our data to send or modify</param>
        /// <returns>Returns the response from the API containing our result code and extra display data.</returns>
        public async Task<HttpResponseMessage> GetAsync(IAPIPayload payload)
        {
            using (var client = new HttpClient())
            {
                await RefreshTokenIfExpired();

                client.SetBearerToken(_token.AccessToken);
                var response = await client.GetAsync($"{URI}{CLIENT_ENDPOINT}{payload.ToGetAPIString()}");

                return response;
            }
        }

        /// <summary>
        /// Standard Post call to our api using a specific payload containing an object to
        /// use inside our endpoint.
        /// </summary>
        /// <param name="payload">Contains all our data to send or modify</param>
        /// <returns>Returns the response from the API containing our result code and extra display data.</returns>
        public async Task<HttpResponseMessage> PostAsync(IAPIPayload payload)
        {
            using (var client = new HttpClient())
            {
                await RefreshTokenIfExpired();

                client.SetBearerToken(_token.AccessToken);
                var response = await client.PostAsync($"{URI}{CLIENT_ENDPOINT}{payload.ToPostAPIString()}", payload);

                return response;
            }
        }

        /// <summary>
        /// Standard Put call to our api using a specific payload containing an object to
        /// use inside our endpoint.
        /// </summary>
        /// <param name="payload">Contains all our data to send or modify</param>
        /// <returns>Returns the response from the API containing our result code and extra display data.</returns>
        public async Task<HttpResponseMessage> PutAsync(IAPIPayload payload)
        {
            using (var client = new HttpClient())
            {
                await RefreshTokenIfExpired();

                client.SetBearerToken(_token.AccessToken);
                var response = await client.PutAsync($"{URI}{CLIENT_ENDPOINT}{payload.ToPutAPIString()}", payload);

                return response;
            }
        }

        /// <summary>
        /// Standard Delete call to our api using a specific payload containing an object to
        /// use inside our endpoint.
        /// </summary>
        /// <param name="payload">Contains all our data to send or modify</param>
        /// <returns>Returns the response from the API containing our result code and extra display data.</returns>
        public async Task<HttpResponseMessage> DeleteAsync(IAPIPayload payload)
        {
            using (var client = new HttpClient())
            {
                await RefreshTokenIfExpired();

                client.SetBearerToken(_token.AccessToken);
                var response = await client.DeleteAsync($"{URI}{CLIENT_ENDPOINT}{payload.ToDeleteAPIString()}", payload);

                return response;
            }
        }

        /// <summary>
        /// Checks if our Access Token is expired or null. If so, it requests a new one from our identity server.
        /// </summary>
        private async Task RefreshTokenIfExpired()
        {
            if (_token == null || IsTokenExpired)
                _token = await GetAccessToken();
        }

        /// <summary>
        /// Sends a request to our Identity Server to claim an Access Token. This token gives access to our
        /// api resources that are managed by the Server.
        /// </summary>
        /// <returns>Returns a TokenResponse which contains all info needed to consume the API</returns>
        private async Task<TokenResponse> GetAccessToken()
        {
            var client = new DiscoveryClient(IDENTITY_URI);
            client.Policy.RequireHttps = false;

            var openIdConnectEndPoint = await client.GetAsync();
            if (openIdConnectEndPoint.TokenEndpoint == null)
                throw new ErrorCodeException("0001", "Failed to retrieve TokenEndpoint from IdentityServer. It's most likely offline!", new ArgumentNullException());

            var tokenClient = new TokenClient(openIdConnectEndPoint.TokenEndpoint, _botCredentials.ApiClientID, _botCredentials.ApiClientSecret);
            var accessToken = await tokenClient.RequestClientCredentialsAsync(JHINBOT_API);

            _tokenExpiresTime = DateTimeOffset.Now.ToUnixTimeSeconds() + accessToken.ExpiresIn;

            return accessToken;
        }
    }
}
