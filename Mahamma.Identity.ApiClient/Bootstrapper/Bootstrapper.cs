using Mahamma.Identity.ApiClient.AppService;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Identity.ApiClient.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient
{
    public static class Bootstrapper
    {
        public static void ResolveMahammaIdentityClientApiService(this IServiceCollection services, IConfiguration configuration, string AppSettings = "", string httpSettingKey = "")
        {
            Base.HttpRequest.Bootstrapper.ResolveHttpRequestService(services, configuration, httpSettingKey);
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();

            MahammaIdentityClientApiSetting setting = new();
            if (!string.IsNullOrWhiteSpace(AppSettings))
            {
                configuration.Bind(AppSettings, setting);
            }
            services.AddSingleton(setting);
        }
    }
}
