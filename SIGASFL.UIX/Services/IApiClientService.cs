using SIGASFL.Models;

namespace SIGASFL.UIX.Services
{
    public interface IApiClientService
    {
        string apiUrl { get; }
        Task<ClientResponse<TResponse>> GetStringAsync<TResponse>(string endPoint);
        Task<ClientResponse<TResponse>> GetFromJsonAsync<TResponse>(string endPoint);
        Task<ClientResponse<TResponse>> PostAsJsonAsync<TResponse, TValue>(string endpoint, TValue value);
        Task<ClientResponse<TResponse>> PutAsJsonAsync<TResponse, TValue>(string endpoint, TValue value);
        Task<ClientResponse<TResponse>> DeleteAsync<TResponse, TValue>(string endpoint);
    }
}
