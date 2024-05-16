using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SIGASFL.Models;

namespace SIGASFL.UIX.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions serializerOptions = new() { PropertyNameCaseInsensitive = true };
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        const string _authKey = "AUTH_INFO";

        public string apiUrl => "https://localhost:7032/";
        //public string apiUrl => "https://api-SIGASFL.fvtech.net/";

        public ApiClientService(AuthenticationStateProvider authenticationStateProvider)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiUrl),
            };

            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ClientResponse<TResponse>> GetStringAsync<TResponse>(string endPoint)
        {
            var result = new ClientResponse<TResponse>();
            try
            {
                await SetAuthHeader();

                var jsonObject = await httpClient.GetStringAsync(endPoint);
                result = JsonSerializer.Deserialize<ClientResponse<TResponse>>(jsonObject, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                    await LogoutAsync();
                else
                    CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref result, ex.Message);
            }

            return result;
        }

        public async Task<ClientResponse<TResponse>> GetFromJsonAsync<TResponse>(string endPoint)
        {
            var result = new ClientResponse<TResponse>();
            try
            {
                await SetAuthHeader();
                result = await httpClient.GetFromJsonAsync<ClientResponse<TResponse>>(endPoint, options: serializerOptions);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                    await LogoutAsync();
                else
                    CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref result, ex.Message);
            }

            return result;
        }

        public async Task<ClientResponse<TResponse>> PostAsJsonAsync<TResponse, TValue>(string endpoint, TValue value)
        {
            await SetAuthHeader();
            var result = await GetFromHttpResponse<TResponse>(await httpClient.PostAsJsonAsync<TValue>(endpoint, value));

            if (result != null)
            {
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    await LogoutAsync();
            }

            return result;
        }

        public async Task<ClientResponse<TResponse>> PutAsJsonAsync<TResponse, TValue>(string endpoint, TValue value)
        {
            await SetAuthHeader();
            var result = await GetFromHttpResponse<TResponse>(await httpClient.PutAsJsonAsync<TValue>(endpoint, value));
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                await LogoutAsync();

            return result;
        }

        public async Task<ClientResponse<TResponse>> DeleteAsync<TResponse, TValue>(string endpoint)
        {
            await SetAuthHeader();
            var result = (await GetFromHttpResponse<TResponse>(await httpClient.DeleteAsync(endpoint)));

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                await LogoutAsync();

            return result;
        }

        private async Task<ClientResponse<TResponse>> GetFromHttpResponse<TResponse>(HttpResponseMessage httpResponse)
        {
            var response = new ClientResponse<TResponse>
            {
                StatusCode = httpResponse.StatusCode
            };

            if (!httpResponse.IsSuccessStatusCode)
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response);

            var stringContent = await httpResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(stringContent))
                CommonMessage.SetMessage(CommonMessage.ERROR_BAD_REQUEST, ref response); //todo: crear error de no content.

            try
            {
                response = JsonSerializer.Deserialize<ClientResponse<TResponse>>(stringContent, serializerOptions);
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, ex.Message);
            }
            return response;
        }

        private async Task SetAuthHeader()
        {
            /*if (await LocalStorageService.ContainKeyAsync(_authKey))
            {
                try
                {
                    var user = await LocalStorageService.GetItemAsync<LoginResponse>(_authKey);
                    if (user != null)
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", user?.Token);
                    }
                }
                catch (Exception ex)
                {
                }
            }*/
        }

        private async Task LogoutAsync()
        {
            //await LocalStorageService.RemoveItemAsync(_cwKey);
            //await LocalStorageService.RemoveItemAsync(_authKey);
            //((LocalAuthenticationStateProvider)_authenticationStateProvider).LogoutNotification();
        }
    }
}
