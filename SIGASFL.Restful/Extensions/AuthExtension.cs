using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SIGASFL.Restful.Extensions
{
    public static class AuthExtension
    {
        public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSettings = configuration.GetSection("TokenSettings");
            var key = Encoding.ASCII.GetBytes(tokenSettings.GetValue<string>("SecretKey"));
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = false,

                };
            });
        }
    }
}
