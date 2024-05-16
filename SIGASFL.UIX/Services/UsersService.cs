using Microsoft.AspNetCore.Components.Authorization;
using SIGASFL.Models.Contracts.Request;
using SIGASFL.Models.Contracts.Response;
using SIGASFL.Models;
using SIGASFL.Models.Views;
using System.Text.Encodings.Web;

namespace SIGASFL.UIX.Services
{
    public class UsersService : BaseApiService
    {
        //private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        const string _authKey = "AUTH_INFO";
        public UsersService(IApiClientService api, 
            //ILocalStorageService localStorageService, 
            AuthenticationStateProvider authenticationStateProvider) : base(api)
        {
            //_localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ClientResponse<ProfileResponse>> CreateAccount(LoginRequest loginRequest)
        {
            var r = await Api.PostAsJsonAsync<ProfileResponse, LoginRequest>($"/api/User/CreateAccount", loginRequest);
            return r;
        }

        public async Task<ClientResponse<List<TwoFactorAuthenticatorView>>> TFAByUser(string UserId)
        {
            var r = await Api.GetFromJsonAsync<List<TwoFactorAuthenticatorView>>($"/api/User/TFAByUser?UserId={UserId}");
            return r;
        }
        public async Task<ClientResponse<bool>> TFARemoveByUser(string Id)
        {
            var r = await Api.DeleteAsync<bool, string>($"/api/User/TFARemoveByUser?Id={Id}");
            return r;
        }

        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest loginInfo)
        {
            var response = await Api.PostAsJsonAsync<LoginResponse, LoginRequest>($"/api/User/Login", loginInfo);
            if (response.IsSuccess)
            {
                //await _localStorageService.SetItemAsync(_authKey, response.Data);
                await _authenticationStateProvider.GetAuthenticationStateAsync();
                //await _localStorageService.SetItemAsync(_cwKey, cwResponse.Data.token);
            }

            return response;
        }

        public async Task<ClientResponse<bool>> ValidatePassword(LoginRequest loginInfo)
        {
            var response = await Api.PostAsJsonAsync<bool, LoginRequest>($"/api/User/ValidatePassword", loginInfo);
            return response;
        }

        public async Task<ClientResponse<bool>> ForgetPassword(string Email)
        {
            var response = await Api.GetFromJsonAsync<bool>($"/api/User/ForgetPassword?Email={UrlEncoder.Default.Encode(Email)}");
            return response;
        }

        public async Task<ClientResponse<LoginResponse>> OTPVerification(string UserId, int TFAId, string RequestedBy)
        {
            var response = await Api.GetFromJsonAsync<LoginResponse>($"/api/User/OTPVerification?UserId={UserId}&TFAId={TFAId}&RequestedBy={RequestedBy}");
            return response;
        }

        public async Task<ClientResponse<bool>> OTPValidation(string UserId, int TFAId, string Token)
        {
            var response = await Api.GetFromJsonAsync<bool>($"/api/User/OTPValidation?UserId={UserId}&TFAId={TFAId}&Token={Token}");
            return response;
        }

        public async Task LogoutAsync()
        {
            //await _localStorageService.RemoveItemAsync(_authKey);
            ((LocalAuthenticationStateProvider)_authenticationStateProvider).LogoutNotification();
        }

        public async Task<ClientResponse<LoginResponse>> SetAUTH(LoginResponse loginResponse)
        {
            var response = new ClientResponse<LoginResponse>();
            //await _localStorageService.SetItemAsync(_authKey, loginResponse);
            await _authenticationStateProvider.GetAuthenticationStateAsync();

            return response;
        }

        public async Task<ClientResponse<bool>> TwoFA(SecurityOptionRequest som)
        {
            var r = await Api.PostAsJsonAsync<bool, SecurityOptionRequest>($"/api/User/TwoFA", som);
            return r;
        }

        public async Task<ClientResponse<LoginResponse>> ChangePassword(PWDRequest model)
        {
            var response = await Api.PostAsJsonAsync<LoginResponse, PWDRequest>($"/api/User/ChangePassword", model);
            return response;
        }

        public async Task<ClientResponse<LoginResponse>> UpdatePassword(PWDRequest model)
        {
            var response = await Api.PutAsJsonAsync<LoginResponse, PWDRequest>($"/api/User/UpdatePassword", model);
            return response;
        }
    }
}
