using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGASFL.Helpers;
using SIGASFL.Models.Contracts.Request;
using SIGASFL.Models.Contracts.Response;
using SIGASFL.Models;
using SIGASFL.Services.Interface;
using SIGASFL.Models.Views;

namespace SIGASFL.Restful.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUsersService userService;
        private readonly INotificationService notificationService;
        private readonly IStoredProcedureService storedProcedureService;
        protected readonly IConfiguration configuration;
        public UserController(ILogger<UserController> logger, IUsersService userService, INotificationService notificationService,
            IStoredProcedureService storedProcedureService, IConfiguration configuration)
        {
            _logger = logger;
            this.userService = userService;
            this.notificationService = notificationService;
            this.storedProcedureService = storedProcedureService;
            this.storedProcedureService = storedProcedureService;
            this.configuration = configuration;
        }


        [HttpPost, Route("Create"), AllowAnonymous]
        public async Task<ClientResponse<ProfileResponse>> CreateAccount(LoginRequest loginRequest)
        {
            var r = await userService.CreateAccount(loginRequest);
            return r;
        }

        [HttpPost, Route("Login"), AllowAnonymous]
        public async Task<ClientResponse<LoginResponse>> Authenticate(LoginRequest loginRequest)
        {
            var response = await userService.Login(loginRequest);
            return response;
        }

        [HttpPost, Route("ValidatePassword")]
        public async Task<ClientResponse<bool>> ValidatePassword(LoginRequest loginRequest)
        {
            var response = await userService.ValidatePassword(loginRequest);
            return response;
        }

        [HttpGet, Route("ChangePicture")]
        public async Task<ClientResponse<bool>> ChangePicture(string UserId, string FileName)
        {
            var response = await userService.ChangePicture(UserId, FileName);
            return response;
        }

        [HttpGet, Route("Add2FA")]
        public async Task<ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>> Add2FA(string UserId, int TFAId)
        {
            var response = await userService.Add2FA(UserId, TFAId);

            return response;
        }

        [HttpGet, Route("TFAByUser")]
        public async Task<ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>> GetTFAByUser(string UserId)
        {
            var response = await userService.GetTFAByUser(UserId);

            return response;
        }

        [HttpDelete, Route("TFARemoveByUser")]
        public async Task<ClientResponse<bool>> TFARemoveByUser(string Id)
        {
            var response = await userService.TFARemoveByUser(Id);
            return response;
        }

        [HttpGet, Route("ForgetPassword"), AllowAnonymous]
        public async Task<ClientResponse<bool>> ForgetPassword(string Email)
        {
            var response = new ClientResponse<bool>();

            var link = Helpers.Utils.RandomString(127);
            var code = $"{Helpers.Utils.RandomString(3)}-{Helpers.Utils.RandomString(3)}";

            response = await userService.PasswordReset(Email, link, code);

            if (response.IsSuccess && response.Data)
            {
                //var request = await Utils.SendGrid.Requests.Password_Reset(Email, configuration.GetValue<string>("BaseURLSettings:Url") + "reset-password/" + link, code);
                //response.IsSuccess = request.IsSuccessStatusCode;
            }

            return response;
        }

        [HttpGet, Route("OTPVerification"), AllowAnonymous]
        public async Task<ClientResponse<LoginResponse>> OTPVerification(string UserId, int TFAId, string RequestedBy)
        {
            var response = new ClientResponse<LoginResponse>();

            var code = $"{Helpers.Utils.RandomString(4).ToUpper()}-{Helpers.Utils.RandomString(4).ToUpper()}";

            response = await userService.UserVerification(UserId, TFAId, RequestedBy, code);
            if (response.IsSuccess && response.Data != null)
            {
                switch (TFAId)
                {
                    case 1:
                        //SmallTitle = $"Please type the verification code displayed on your FH Authenticator App.";
                        break;
                    case 2:
                        //SmallTitle = $"We are calling you at the phone number ending in {FH.Helpers.Utils.MaskPhoneNumber(user.PhoneNumber)}.";
                        break;
                    case 3:
                        //SmallTitle = $"We have sent you a OTP Code to the following Email ({FH.Helpers.Utils.MaskEmailAddress(user.Email)}).";
                        //await Utils.SendGrid.Requests.Multi_Factor_Auth_Code(RequestedBy, response.Data.DisplayName, configuration.GetValue<string>("BaseURLSettings:Url") + "2fa/verify", code);
                        break;
                    case 4:
                        await notificationService.SendSMS(RequestedBy, $"FH: {code} is your verification code. Never share this code with anyone. We will never ask for this code over the phone, social media, or other mediums.");
                        //SmallTitle = $"We are sent a SMS Verification Code to {FH.Helpers.Utils.MaskPhoneNumber(user.PhoneNumber)}.";
                        break;
                    case 5:
                        //SmallTitle = $"Enter the answer to your secret question.";
                        break;
                    case 6:
                        //SmallTitle = $"Enter one of the backup codes that we have provided.";
                        break;
                }
            }

            return response;
        }

        [HttpGet, Route("OTPValidation"), AllowAnonymous]
        public async Task<ClientResponse<bool>> OTPValidation(string UserId, int TFAId, string Token)
        {
            var response = new ClientResponse<bool>();

            response = await userService.UserValidation(UserId, TFAId, Token);

            return response;
        }

        [HttpPost, Route("TwoFA")]
        public async Task<ClientResponse<bool>> TwoFA(SecurityOptionRequest som)
        {
            var response = await userService.TwoFA(som);
            return response;
        }

        [HttpPost, Route("ChangePassword")]
        public async Task<ClientResponse<LoginResponse>> ChangePassword(PWDRequest model)
        {
            var response = await userService.ChangePassword(model);
            if (response.IsSuccess && response.Data != null)
            {
                //await Utils.SendGrid.Requests.Password_Changed(response.Data.Email, response.Data.DisplayName, _configuration.GetValue<string>("BaseURLSettings:Url") + "change-password");
            }
            return response;
        }

        [HttpPut, Route("UpdatePassword"), AllowAnonymous]
        public async Task<ClientResponse<LoginResponse>> UpdatePassword(PWDRequest model)
        {
            var response = await userService.UpdatePassword(model);
            if (response.IsSuccess && response.Data != null)
            {
                //await Utils.SendGrid.Requests.Password_Changed(response.Data.Email, response.Data.DisplayName, configuration.GetValue<string>("BaseURLSettings:Url") + "update-password");
            }
            return response;
        }

        [HttpGet, Route("BackupCodes")]
        public async Task<ClientResponse<List<string>>> BackupCodes(string UserId)
        {
            var response = await userService.BackupCode(UserId);

            return response;
        }
    }
}
