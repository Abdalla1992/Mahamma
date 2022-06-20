using Mahamma.Helper.EmailSending.IService;
using Mahamma.Helper.EmailSending.Service;
using Mahamma.Helper.EmailSending.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mahamma.Helper.EmailSending.Bootstrapper
{
    public static class Bootstrapper
    {
        public static void ResolveEmailSendingService(this IServiceCollection services, IConfiguration configuration, string settingKey = "")
        {
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            SMTPSettings setting = new();
            if (!string.IsNullOrWhiteSpace(settingKey))
            {
                configuration.Bind(settingKey, setting);
            }
            services.AddSingleton(setting);
        }
    }
}
