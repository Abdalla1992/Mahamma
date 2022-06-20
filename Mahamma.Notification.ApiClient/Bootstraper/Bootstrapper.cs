using Mahamma.Notification.ApiClient.AppService;
using Mahamma.Notification.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.ApiClient.Bootstraper
{
    public static class Bootstrapper
    {
        public static void ResolveMahammaNotificationClientApiService(this IServiceCollection services, IConfiguration configuration, string AppSettings = "", string httpSettingKey = "")
        {
            Base.HttpRequest.Bootstrapper.ResolveHttpRequestService(services, configuration, httpSettingKey);
            services.AddScoped<INotificationService, NotificationService>();

            MahammaNotificationClientApiSetting setting = new();
            if (!string.IsNullOrWhiteSpace(AppSettings))
            {
                configuration.Bind(AppSettings, setting);
            }
            services.AddSingleton(setting);
        }
    }
}
