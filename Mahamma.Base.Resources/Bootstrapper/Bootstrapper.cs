using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Resources.Bootstrapper
{
    public static class Bootstrapper
    {
        public static void ResolveMessages(IServiceCollection services)
        {
            services.AddScoped<IResourceReader.IMessageResourceReader,ResourceReader.MessageResourceReader>();
            services.AddScoped<IResourceReader.INotificationResourceReader,ResourceReader.NotificationResourceReader>();
        }

     
        public static void ResolveNotificationMessages(IServiceCollection services)
        {
            services.AddScoped<IResourceReader.INotificationResourceReader, ResourceReader.NotificationResourceReader>();
        }
    }
}
