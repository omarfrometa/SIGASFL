using System.Text;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;
using SIGASFL.Repositories.Implementation;
using SIGASFL.Repositories.Interface;
using SIGASFL.Services.Interface;
using SIGASFL.Services.Implementation;
using SIGASFL.Services.Mapper;
using SIGASFL.Models;

namespace SIGASFL.Restful.Extensions
{
    public static class ServicesExtension
    {
        public static void AddCommonServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IStoredProcedureService, StoredProcedureService>();

            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<ICustomMapper, CustomMapper>();
            //services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRolesService, RolesService>();

            var nc = config.GetSection("Notifications").Get<NotificationConfig>();
            services.AddSingleton<INotificationService>(new NotificationService(nc));

            services.AddScoped(typeof(DbService<,,>));
        }

        public static DateTime GetCreationTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            var creationTime = File.GetLastWriteTimeUtc(filePath);

            var tz = target ?? TimeZoneInfo.Local;
            creationTime = TimeZoneInfo.ConvertTimeFromUtc(creationTime, tz);

            return creationTime;
        }

        public static string GetRemoteIpAddress(this HttpContext context)
        {
            var result = string.Empty;
            if (context != null && context.Connection != null && context.Connection.RemoteIpAddress != null)
            {
                result = context.Connection.RemoteIpAddress.ToString();
            }
            return result;
        }

        public static string GetActionDescription(this ActionDescriptor actionDescriptor)
        {
            var result = string.Empty;
            if (actionDescriptor != null)
            {
                if (actionDescriptor.AttributeRouteInfo != null && !string.IsNullOrEmpty(actionDescriptor.AttributeRouteInfo.Template))
                {
                    result = actionDescriptor.AttributeRouteInfo.Template;
                }
            }
            return result;
        }

        public static async Task<string> ReadRequestBodyAsync(this HttpRequest request)
        {
            request.EnableBuffering();
            if (request.Body.Position == request.ContentLength)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        public static string ReadRequestBody(this HttpRequest request)
        {
            request.EnableBuffering();
            if (request.Body.Position == request.ContentLength)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            request.Body.Read(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }
    }
}
