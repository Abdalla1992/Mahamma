using Mahamma.Integration.Meeting.Zoom.IService;
using Mahamma.Integration.Meeting.Zoom.Service;
using Mahamma.Integration.Meeting.Zoom.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Mahamma.Integration.Meeting.Zoom.Bootstrapper
{
    public static class Bootstrapper
    {
        public static void ResolveMeetingZoomService(this IServiceCollection services, IConfiguration configuration,string AppSettings = "",string httpSettingKey="")
        {
            Base.HttpRequest.Bootstrapper.ResolveHttpRequestService(services, configuration, httpSettingKey);
            services.AddScoped<IMeetingService, MeetingService>();

            MeetingZoomSetting setting = new();
            if (!string.IsNullOrWhiteSpace(AppSettings))
            {
                configuration.Bind(AppSettings, setting);
            }
            services.AddSingleton(setting);
        }
    }
}
