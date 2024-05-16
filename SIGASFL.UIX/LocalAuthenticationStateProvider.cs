using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SIGASFL.UIX
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        //private readonly ILocalStorageService _localStorageService;
        const string _authKey = "AUTH_INFO";
        public LocalAuthenticationStateProvider()//ILocalStorageService localStorageService)
        {
            //_localStorageService = localStorageService;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            /*try
            {
                if (await _localStorageService.ContainKeyAsync(_authKey))
                {
                    var loginInfo = await _localStorageService.GetItemAsync<LoginResponse>(_authKey);

                    var claims = new[]
                    {
                    new Claim("UserId", loginInfo.Id.ToString()),
                    //new Claim(ClaimTypes.Name, loginInfo.DisplayName),
                    new Claim(ClaimTypes.Email, loginInfo.Email),
                    new Claim("Token", loginInfo.Token),
                    //new Claim("PictureUrl", loginInfo.PictureURL),
                    //new Claim("RoleName", loginInfo.UserTypeName.ToString()),
                    //new Claim(ClaimTypes.Role, loginInfo.UserTypeId.ToString())
                };

                    var identity = new ClaimsIdentity(claims, "UserInfo");
                    var state = new AuthenticationState(new ClaimsPrincipal(identity));
                    NotifyAuthenticationStateChanged(Task.FromResult(state));
                    return state;
                }
            }
            catch (Exception ex)
            {

            }
            */
            return new AuthenticationState(new ClaimsPrincipal());
        }

        public void LogoutNotification()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}
