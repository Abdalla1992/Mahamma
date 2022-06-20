using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Reflection;
using Mahamma.Document.Api.Infrastructure.AutofacHandler;
using Mahamma.Document.AppService.Document.Helper;
using Mahamma.Document.Api.Infrastructure.Behaviors;
using System.Linq;
using System.Collections.Generic;
using Mahamma.Document.AppService.Document.Settings;

namespace Mahamma.Document.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private string AllowedOrigins { get; } = "AllowedOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region CORS
            List<string> origns = new List<string>();

            IConfigurationSection originsSection = Configuration.GetSection("AllowedOrigins");
            string[] appSettingOrigns = originsSection.AsEnumerable().Where(s => s.Value != null).Select(a => a.Value).ToArray();

            foreach (string k in appSettingOrigns)
            {
                origns.AddRange(k.Split(",").ToList());
            }

            Log.Warning($"Origins: {string.Join(" , ", origns)}");

            services.AddCors(options =>
            {
                options.AddPolicy(AllowedOrigins,
                    builder =>
                    {
                        builder.WithOrigins(origns.ToArray())
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowAnyOrigin();
                               //.AllowCredentials();
                    });
            });
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mahamma.Document.Api", Version = "v1" });
                c.OperationFilter<SwaggerFileOperationFilter>();
            });

            JWTAppSettingsConfig(services);
            UploadAppSettingsConfig(services);
            UserProfileImageAppSettingsConfig(services);

            services.AddScoped<IFileHelper, FileHelper>();

            ApiClient.Bootstrapper.ResolveMahammaClientApiService(services, Configuration, AppSettings: "ClientApiSettings", httpSettingKey: "HttpRequestSettings");
            Mahamma.Identity.ApiClient.Bootstrapper.ResolveMahammaIdentityClientApiService(services, Configuration, AppSettings: "ClientApiSettings", httpSettingKey: "HttpRequestSettings");
            #region Auto fac
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            //container.RegisterModule(new SettingsModule());
            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mahamma.Document.Api v1"));
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            #region CORS
            app.UseCors(AllowedOrigins);
            #endregion
            app.UseMiddleware<Infrastructure.Middleware.JWTMiddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IServiceCollection JWTAppSettingsConfig(IServiceCollection services)
        {
            JWTSetting jWTSetting = new();
            Configuration.Bind("JWTSetting", jWTSetting);
            services.AddSingleton(jWTSetting);

            return services;
        }
        private IServiceCollection UploadAppSettingsConfig(IServiceCollection services)
        {
            UploadSetting uploadSetting = new();
            Configuration.Bind("UploadSetting", uploadSetting);
            services.AddSingleton(uploadSetting);

            return services;
        }

        private IServiceCollection UserProfileImageAppSettingsConfig(IServiceCollection services)
        {
            UserProfileImageSetting userProfileImageSetting = new();
            Configuration.Bind("UserProfileImageSetting", userProfileImageSetting);
            services.AddSingleton(userProfileImageSetting);

            return services;
        }
    }
}
