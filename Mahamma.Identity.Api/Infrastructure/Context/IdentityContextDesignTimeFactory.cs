using Mahamma.Identity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mahamma.Identity.Api.Infrastructure.Context
{
    public class IdentityContextDesignTimeFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

            optionsBuilder.UseSqlServer(GetConnectionString(), options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));
            
            return new IdentityContext(optionsBuilder.Options);
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
