using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Models.Contracts.Request;
using SIGASFL.Models.Contracts.Response;
using SIGASFL.Models.Views;

namespace SIGASFL.Services.Interface
{
    public interface IUsersService
    {
        Task<ClientResponse<ProfileResponse>> CreateAccount(LoginRequest loginRequest);
        Task<ClientResponse<LoginResponse>> Login(LoginRequest loginRequest);
        Task<ClientResponse<bool>> ValidatePassword(LoginRequest loginRequest);
        Task<ClientResponse<bool>> ChangePicture(string UserId, string FileName);
        Task<ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>> Add2FA(string UserId, int TFAId);
        Task<ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>> GetTFAByUser(string UserId);
        Task<ClientResponse<bool>> PasswordReset(string Email, string Link, string Code);
        Task<ClientResponse<bool>> TwoFA(SecurityOptionRequest som);
        Task<ClientResponse<bool>> TFARemoveByUser(string Id);
        Task<ClientResponse<LoginResponse>> UserVerification(string UserId, int TFAId, string RequestedBy, string Code);
        Task<ClientResponse<bool>> UserValidation(string UserId, int TFAId, string Token);
        Task<ClientResponse<LoginResponse>> ChangePassword(PWDRequest model);
        Task<ClientResponse<LoginResponse>> UpdatePassword(PWDRequest model);
        Task<ClientResponse<List<string>>> BackupCode(string UserId);
    }
}
