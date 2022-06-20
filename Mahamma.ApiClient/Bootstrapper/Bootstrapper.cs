using Mahamma.ApiClient.AppService;
using Mahamma.ApiClient.Interface;
using Mahamma.ApiClient.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.ApiClient
{
    public static class Bootstrapper
    {
        public static void ResolveMahammaClientApiService(this IServiceCollection services, IConfiguration configuration, string AppSettings = "", string httpSettingKey = "")
        {
            Base.HttpRequest.Bootstrapper.ResolveHttpRequestService(services, configuration, httpSettingKey);
            services.AddScoped<ICompanyService, CompanyService>();

            MahammaClientApiSetting setting = new();
            if (!string.IsNullOrWhiteSpace(AppSettings))
            {
                configuration.Bind(AppSettings, setting);
            }
            services.AddSingleton(setting);
        }
    }
}
