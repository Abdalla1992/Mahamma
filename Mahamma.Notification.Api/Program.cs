using Autofac.Extensions.DependencyInjection;
using Mahamma.Api.Infrastructure.Hosting;
using Mahamma.Notification.Api;
using Mahamma.Notification.Infrastructure.Context;
using Mahamma.Notification.Infrastructure.MigrationSetting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

var configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    CreateHostBuilder(args).Build().Run();

    //Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);
    //host.MigrateDbContext<NotificationContext>((context, services) =>
    //{
    //context.Database.Migrate();
    //MigrationScripts.ApplyDbScripts(Path.Combine("Migrations", "FX"), context);
    //MigrationScripts.ApplyDbScripts(Path.Combine("Migrations", "SP"), context);
    //});

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                    .CaptureStartupErrors(false);
                });

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    var logPath = Path.Combine(configuration["Serilog:LogPath"], ".log");
    int.TryParse(configuration["Serilog:FileLogLevel"], out int fileLogLevel);
    int.TryParse(configuration["Serilog:RollingInterval"], out int rollingInterval);

    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console(LogEventLevel.Verbose)
        .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
        .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
        .ReadFrom.Configuration(configuration)
        .WriteTo.File(logPath, (LogEventLevel)fileLogLevel, rollingInterval: (RollingInterval)rollingInterval)
        .CreateLogger();
}

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

public partial class Program
{
    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}

//namespace Mahamma.Notification.Api
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}
