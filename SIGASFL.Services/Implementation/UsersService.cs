using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SIGASFL.Entities;
using SIGASFL.Models.Contracts.Request;
using SIGASFL.Models.Contracts.Response;
using SIGASFL.Models;
using SIGASFL.Repositories;
using SIGASFL.Services.Interface;
using SIGASFL.Services.Mapper;
using SIGASFL.Models.Views;

namespace SIGASFL.Services.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationContext db;
        private readonly ICustomMapper customMapper;
        private readonly IConfiguration _configuration;
        private readonly ITokenManager _tokenManager;
        private readonly string _secretSaltResetToken;
        private readonly string _secretSaltRegisterToken;
        private readonly string _userCreatedDateFormat = "yyyyMMddhhmmss.fff";
        private readonly double _tokenExpirationMinutes = 1440;
        public string ProfileClaimKey => "profile";

        public UsersService(ApplicationContext context, ICustomMapper mapper, IConfiguration configuration, ITokenManager tokenManager)
        {
            db = context;
            customMapper = mapper;
            _configuration = configuration;
            _tokenManager = tokenManager;
        }

        public async Task<ClientResponse<ProfileResponse>> CreateAccount(LoginRequest loginRequest)
        {
            var response = new ClientResponse<ProfileResponse>();

            if (string.IsNullOrEmpty(loginRequest.Email))//TODO NEED + Allowed in EMAIL  || !FH.Helpers.Utils.IsValidEmail(loginRequest.Email))
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_INVALID_EMAIL, ref response);
                return response;
            }

            if (string.IsNullOrEmpty(loginRequest.Password))
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_PASSWORD_REQUIRED, ref response);
                return response;
            }

            byte[] newHash;
            byte[] newSalt;
            Helpers.Utils.CreatePasswordHash(loginRequest.Password, out newHash, out newSalt);

            //Creating User
            var user = new Users
            {
                Id = Guid.NewGuid().ToString(),
                Username = loginRequest.Email,
                Email = loginRequest.Email,
                EmailConfirmed = false,
                PasswordHash = newHash,
                PasswordSalt = newSalt,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                TwoFactorSecretKey = Guid.NewGuid().ToString().ToUpper().Replace("-", "")[..10],
                AllowAccessFailed = false,
                AccessFailedCount = 10,
                ChangePwdNextLogin = false,
                DefaultLanguage = "ENG",
                DisplayName = "Unknown",
                Locked = false,
                ProfileCompleted = false,
                CreatedDate = DateTime.Now
            };

            var profile = new UserProfile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FirstName = "Unknown",
                LastName1 = "Unknown"
            };

            user.UserProfile.Add(profile);

            db.Add(user);
            var rows = await db.SaveChangesAsync();

            if (rows <= 0)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_CREATED, ref response);
                return response;
            }

            response.Code = 0;
            response.IsSuccess = true;
            response.Data = customMapper.Map<ProfileResponse>(user);
            return response;
        }

        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var loginResult = true;
            var response = new ClientResponse<LoginResponse>();
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    SaveHistory(loginRequest, false);
                    return response;
                }

                var user = await db.Users
                                        .Include(u => u.UserProfile)
                                        .FirstOrDefaultAsync(w => w.Email.ToLower().Trim().Equals(loginRequest.Email.ToLower().Trim()));

                if (user == null)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    SaveHistory(loginRequest, false);
                    return response;
                }

                var result = Helpers.Utils.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt);
                if (!result)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    SaveHistory(loginRequest, false);
                    return response;
                }

                if (response.Code.Equals(0))
                {
                    response.Data = customMapper.Map<LoginResponse>(user);

                    user.LastAccessDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(loginRequest.LastIpAddress))
                    {
                        user.LastIpAddress = loginRequest.LastIpAddress;
                    }

                    db.Update(user).State = EntityState.Modified;
                    db.Update(user).Property(x => x.Seq).IsModified = false;

                    await db.SaveChangesAsync();

                    var secretKey = _configuration.GetSection("TokenSettings")["SecretKey"];
                    double expirationInMinutes = Convert.ToDouble(_configuration.GetSection("TokenSettings")["ExpireInMinute"]);
                    Dictionary<string, string> loginClaims = new Dictionary<string, string>
                    {
                        { ClaimTypes.NameIdentifier, user.Id.ToString() },
                        { ClaimTypes.Name, user.DisplayName },
                        { ClaimTypes.Email, user.Email },
                        { ProfileClaimKey, JsonSerializer.Serialize(customMapper.Map<ProfileResponse>(user)) }
                    };

                    var tokenResponse = await _tokenManager.GenerateToken(secretKey, expirationInMinutes, loginClaims);

                    if (tokenResponse.IsSuccess)
                        response.Data.Token = tokenResponse.Data;
                    else
                        response.CopyResponse(tokenResponse);
                }

            }
            catch (SqlException sqlEx)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, "Problem for accesing to SQL Database.");
                loginResult = false;
                response.Data = null;
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, "System Exception");
                loginResult = false;
                response.Data = null;
            }

            SaveHistory(loginRequest, loginResult);

            return response;
        }
        public async Task<ClientResponse<bool>> ValidatePassword(LoginRequest loginRequest)
        {
            var response = new ClientResponse<bool>();
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    return response;
                }

                var user = await db.Users
                                        .Include(u => u.UserProfile)
                                        .FirstOrDefaultAsync(w => w.Email.ToLower().Trim().Equals(loginRequest.Email.ToLower().Trim()));

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    return response;
                }

                var result = Helpers.Utils.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt);
                if (!result)
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.IsSuccess = false;
                response.Data = false;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = false;
                return response;
            }

            response.IsSuccess = true;
            response.Data = true;
            return response;
        }

        private void SaveHistory(LoginRequest loginRequest, bool loginResult)
        {
            try
            {
                /*var lh = new LoginHistory
                {
                    UserLogin = loginRequest.Email,
                    UserPwd = loginResult ? "********" : loginRequest.Password,
                    IpAddress = loginRequest.LastIpAddress,
                    CreatedDate = DateTime.Now,
                    LoginResult = loginResult
                };

                db.LoginHistory.Add(lh);*/
                var result = db.SaveChanges() > 0;
            }
            catch { }
        }

        public async Task<ClientResponse<bool>> ChangePicture(string UserId, string FileName)
        {
            var response = new ClientResponse<bool>();

            var entity = await db.Users.FindAsync(UserId);
            if (entity != null)
            {
                entity.Picture = FileName;

                db.Update(entity).State = EntityState.Modified;
                var result = await db.SaveChangesAsync() > 0;

                response.IsSuccess = result;
                response.Data = result;

            }

            return response;
        }

        public async Task<ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>> Add2FA(string UserId, int TFAId)
        {
            var response = new ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>();

            var tfa = new UserTfacross
            {
                Id = Guid.NewGuid().ToString(),
                UserId = UserId,
                Tfaid = TFAId,
                CreatedDate = DateTime.Now
            };

            db.UserTfacross.Add(tfa);
            var result = await db.SaveChangesAsync() > 0;
            response.IsSuccess = result;

            if (result)
            {
                var entities = await db.UserTfacross
                                .AsNoTracking()
                                .Include(x => x.User)
                                .Include(x => x.Tfa)
                                .Where(c => c.UserId == UserId)
                                .ToListAsync();

                response.Data = customMapper.Map<List<TwoFactorAuthenticatorView>>(entities);
            }

            return response;
        }

        public async Task<ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>> GetTFAByUser(string UserId)
        {
            var response = new ClientResponse<IEnumerable<TwoFactorAuthenticatorView>>();

            var entities = await db.UserTfacross
                                .AsNoTracking()
                                .Include(x => x.User)
                                .Include(x => x.Tfa)
                                .Where(c => c.UserId == UserId)
                                .ToListAsync();

            response.Data = customMapper.Map<List<TwoFactorAuthenticatorView>>(entities);

            return response;
        }

        public async Task<ClientResponse<bool>> TFARemoveByUser(string Id)
        {
            var response = new ClientResponse<bool>();

            var record = await db.UserTfacross.FirstOrDefaultAsync(x => x.Id == Id);
            if (record != null)
            {
                db.UserTfacross.Remove(record);
                response.IsSuccess = await db.SaveChangesAsync() > 0;
                response.Data = response.IsSuccess;
            }

            return response;
        }

        public async Task<ClientResponse<bool>> PasswordReset(string Email, string Link, string Code)
        {
            var response = new ClientResponse<bool>();

            if (string.IsNullOrEmpty(Email) || !Helpers.Utils.IsValidEmail(Email))
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_INVALID_EMAIL, ref response);
                return response;
            }

            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Trim().ToLower().Equals(Email.Trim().ToLower()));
            if (user == null)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_USERNAME_NOT_FOUND, ref response);
                return response;
            }

            var upr = new UserForgotPassword
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Link = Link,
                Code = Code,
                CreatedDate = DateTime.Now,
                Verified = false
            };

            db.Entry(upr).State = EntityState.Added;
            db.UserForgotPassword.Add(upr);
            var result = await db.SaveChangesAsync() > 0;

            response.IsSuccess = result;
            response.Data = result;

            return response;
        }

        public async Task<ClientResponse<LoginResponse>> UserVerification(string UserId, int TFAId, string RequestedBy, string Code)
        {
            var response = new ClientResponse<LoginResponse>();

            var user = await db.Users.FindAsync(UserId);
            if (user == null)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                return response;
            }
            /*
            var uv = new UserVerify
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                TwoFactorAuthId = TFAId,
                RequestedBy = RequestedBy,
                Token = Code,
                CreatedDate = DateTime.Now,
                Verified = false
            };

            db.Entry(uv).State = EntityState.Added;
            db.UserVerify.Add(uv);*/
            var result = await db.SaveChangesAsync() > 0;

            response.IsSuccess = result;
            response.Data = customMapper.Map<LoginResponse>(user);

            return response;
        }

        public async Task<ClientResponse<bool>> UserValidation(string UserId, int TFAId, string Token)
        {
            var response = new ClientResponse<bool>();

            if (TFAId == 5) // Security Questions
            {
                var question = await db.UserSecurityQuestion.CountAsync(x => x.UserId == UserId && (x.Answer1.Trim().ToLower() == Token.Trim().ToLower() || x.Answer2.Trim().ToLower() == Token.Trim().ToLower() || x.Answer3.Trim().ToLower() == Token.Trim().ToLower()));
                if (question == 0)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                }

                response.IsSuccess = question > 0;
                response.Data = question > 0;
                return response;
            }
            else if (TFAId == 6) // Backup Codes
            {
                var backupCode = await db.UserTfabackupCode.FirstOrDefaultAsync(x => x.UserId == UserId && x.Code.Trim() == Token.Trim() && !x.Verified);
                if (backupCode == null || string.IsNullOrEmpty(backupCode.Id))
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                    return response;
                }

                backupCode.Verified = true;
                backupCode.VerifiedDate = DateTime.Now;
                db.Update(backupCode).State = EntityState.Modified;
            }
            else
            {
                var uVerify = await db.UserTfaverifiedCode.FirstOrDefaultAsync(x => x.UserId == UserId && x.Tfaid == TFAId && x.Token.Trim() == Token.Trim() && !x.Verified);
                if (uVerify == null || string.IsNullOrEmpty(uVerify.Id))
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                    return response;
                }

                uVerify.Verified = true;
                uVerify.VerifiedDate = DateTime.Now;

                db.Update(uVerify).State = EntityState.Modified;
            }


            var result = await db.SaveChangesAsync() > 0;

            response.IsSuccess = result;
            response.Data = result;

            return response;
        }
        public async Task<ClientResponse<bool>> TwoFA(SecurityOptionRequest som)
        {
            var response = new ClientResponse<bool>();

            var utfa = new UserTfacross();
            utfa.Id = Guid.NewGuid().ToString();
            utfa.UserId = som.UserId;
            utfa.Tfaid = som.TFAId;
            utfa.CreatedDate = DateTime.Now;

            db.UserTfacross.Add(utfa);
            var result = await db.SaveChangesAsync() > 0;

            response.IsSuccess = result;
            response.Data = result;

            return response;
        }

        public async Task<ClientResponse<LoginResponse>> ChangePassword(PWDRequest model)
        {
            var response = new ClientResponse<LoginResponse>();
            try
            {
                if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.CurrentPassword))
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    return response;
                }

                var user = await db.Users.FindAsync(model.UserId);
                if (user == null)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    return response;
                }

                var result = Helpers.Utils.VerifyPasswordHash(model.CurrentPassword, user.PasswordHash, user.PasswordSalt);
                if (!result)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    return response;
                }

                if (response.Code.Equals(0))
                {
                    byte[] newHash;
                    byte[] newSalt;
                    Helpers.Utils.CreatePasswordHash(model.NewPassword, out newHash, out newSalt);
                    user.PasswordHash = newHash;
                    user.PasswordSalt = newSalt;
                    user.LastPwdChangedDate = DateTime.Now;

                    db.Update(user).State = EntityState.Modified;
                    var updated = await db.SaveChangesAsync() > 0;
                    response.IsSuccess = updated;
                    response.Data = customMapper.Map<LoginResponse>(user);
                }
            }
            catch (SqlException sqlEx)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, "Problem for accesing to SQL Database.");
                response.Data = null;
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, "System Exception");
                response.Data = null;
            }
            return response;
        }

        public async Task<ClientResponse<LoginResponse>> UpdatePassword(PWDRequest model)
        {
            var response = new ClientResponse<LoginResponse>();
            try
            {
                if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.NewPassword))
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    return response;
                }

                var user = await db.Users.FindAsync(model.UserId);
                if (user == null)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_AUTH_INVALID_USERNAME_LOGIN, ref response);
                    return response;
                }

                if (response.Code.Equals(0))
                {
                    byte[] newHash;
                    byte[] newSalt;
                    Helpers.Utils.CreatePasswordHash(model.NewPassword, out newHash, out newSalt);
                    user.PasswordHash = newHash;
                    user.PasswordSalt = newSalt;
                    user.LastPwdChangedDate = DateTime.Now;

                    db.Update(user).State = EntityState.Modified;
                    var updated = await db.SaveChangesAsync() > 0;
                    response.IsSuccess = updated;
                    response.Data = customMapper.Map<LoginResponse>(user);
                }
            }
            catch (SqlException sqlEx)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, "Problem for accesing to SQL Database.");
                response.Data = null;
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, "System Exception");
                response.Data = null;
            }
            return response;
        }

        public async Task<ClientResponse<List<string>>> BackupCode(string UserId)
        {
            var response = new ClientResponse<List<string>>();

            var user = await db.Users.FindAsync(UserId);
            if (user == null)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                return response;
            }

            var list = new List<string>();
            for (var i = 0; i < 12; i++)
            {
                try
                {
                    var bcode = Helpers.Utils.RandomString(8);
                    var bc = new UserTfabackupCode
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = UserId,
                        Code = bcode,
                        CreatedDate = DateTime.Now
                    };

                    db.UserTfabackupCode.Add(bc);
                    var result = db.SaveChanges() > 0;
                    if (result)
                    {
                        list.Add(bcode);
                    }
                }
                catch { }
            }

            response.IsSuccess = true;
            response.Data = list;

            return response;
        }
    }
}
