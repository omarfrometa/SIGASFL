using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Services.Interface;

namespace SIGASFL.Services.Implementation
{
    public class TokenManager : ITokenManager
    {
        private readonly string Issuer;
        private readonly string Audience;
        private readonly string SecAlgorithms;
        public TokenManager()
        {
            Issuer = "SIGASFL.pr";
            Audience = "truenorh.pr";
            SecAlgorithms = SecurityAlgorithms.HmacSha256Signature;
        }
        public async Task<ClientResponse<string>> GenerateToken(string secret, double expirationMinutes, Dictionary<string, string> claims)
        {
            var result = new ClientResponse<string>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecAlgorithms),
                NotBefore = DateTime.Now,
                Issuer = Issuer,
                Audience = Audience,
            };

            if (claims != null && claims.Count() > 0)
            {
                tokenDescriptor.Subject = new ClaimsIdentity();
                claims.ToList()
                .ForEach(claim =>
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(claim.Key, claim.Value));
                });
            }

            tokenDescriptor.Expires = DateTime.Now.AddMinutes(expirationMinutes);

            var secToken = tokenHandler.CreateToken(tokenDescriptor);
            result.Data = tokenHandler.WriteToken(secToken);

            return result;
        }

        public async Task<ClientResponse<Dictionary<string, string>>> ValidateToken(string token, string secret)
        {
            var result = new ClientResponse<Dictionary<string, string>>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var creds = new SigningCredentials(key, SecAlgorithms);

            SecurityToken validatedToken;
            var validator = new JwtSecurityTokenHandler();

            // These need to match the values used to generate the token
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidIssuer = Issuer;
            validationParameters.ValidAudience = Audience;
            validationParameters.IssuerSigningKey = key;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateAudience = true;

            if (validator.CanReadToken(token))
            {
                ClaimsPrincipal principal;
                try
                {
                    // This line throws if invalid
                    principal = validator.ValidateToken(token, validationParameters, out validatedToken);

                    // Validar claims
                    if (principal.Claims != null && principal.Claims.Count() > 0)
                    {
                        result.Data = new Dictionary<string, string>();
                        principal.Claims.ToList()
                            .ForEach(claim =>
                            {
                                result.Data.Add(claim.Type, claim.Value);
                            });
                    }
                    else
                        CommonMessage.SetMessage(CommonMessage.ERROR_INVALID_TOKEN, ref result);
                }
                catch (Exception e)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref result, e.Message);
                }
            }
            else
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_INVALID_TOKEN, ref result);
            }

            return result;
        }
    }
}
