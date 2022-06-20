using Mahamma.Base.HttpRequest.IService;
using Mahamma.Base.HttpRequest.Service;
using Mahamma.Base.HttpRequest.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mahamma.Base.HttpRequest
{
    public static class Bootstrapper
    {
        public static void ResolveHttpRequestService(this IServiceCollection services, IConfiguration configuration, string settingKey = "")
        {
            services.AddScoped<IHttpHandler, HttpHandler>();

            HttpRequestSettings setting = new();
            if (!string.IsNullOrWhiteSpace(settingKey))
            {
                configuration.Bind(settingKey, setting);
            }
            services.AddSingleton(setting);
        }

    }
}
