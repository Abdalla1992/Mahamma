using Mahamma.Notification.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Api.Infrastructure.Context
{
    public class MahammaContextDesignTimeFactory : IDesignTimeDbContextFactory<NotificationContext>
    {
        public NotificationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NotificationContext>();

            optionsBuilder.UseSqlServer(GetConnectionString(), options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));
            
            return new NotificationContext(optionsBuilder.Options);
        }

        private static string GetConnectionString()
        {
            var configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"{Directory.GetCurrentDirectory()}/bin/Debug/net5.0/appsettings.json", optional: false, reloadOnChange: true)
              .Build();

            return configurationBuilder["ConnectionString"];
        }
    }
}
