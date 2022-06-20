using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Mahamma.Api.BackGroundService;
using Mahamma.Api.Infrastructure.AutofacHandler;
using Mahamma.Api.Infrastructure.Middleware;
using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.AppService.Task.Helper;
using Mahamma.Infrastructure.AutoMapper;
using Mahamma.Infrastructure.Context;
using Mahamma.Infrastructure.MigrationSetting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mahamma.Api
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
        public void ConfigureServices(IServiceCollection services)
        {
            #region Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MahammaProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Add Controllers
            services.AddControllers();
            #endregion

            #region BackGroundService
            if (bool.TryParse(Configuration["BackGroundServiceSettings:UseBackGroundService"], out bool useBackGroundService) && useBackGroundService)
                services.AddHostedService<MahammaBackGroundService>();
            #endregion

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

            #region AddAuthentication JWT
            // configure jwt authentication            
            byte[] key = Encoding.ASCII.GetBytes(Configuration["JWTSetting:Key"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWTSetting:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWTSetting:Audience"]
                };
            });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mahamma.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Description = "JWT bearer authentication token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Scheme = "Bearer",
                            Description = "JWT bearer authentication token.",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                        },
                        new System.Collections.Generic.List<string>()
                    }
                });
            });
            #endregion

            #region DbContext & Settings
            AddCustomDbContext(services);
            AppSettingsConfig(services);
            ActivitySettings(services);
            JWTAppSettingsConfig(services);
            BackGroundServiceSettingsConfig(services);

            #endregion
            services.AddScoped<Infrastructure.Filter.AuthorizeAttribute>();
            #region ActivityLoggers
            services.AddScoped<ITaskActivityLogger, TaskActivityLogger>();
            services.AddScoped<IProjectActivityLogger, ProjectActivityLogger>();
            #endregion

            Identity.ApiClient.Bootstrapper.ResolveMahammaIdentityClientApiService(services, Configuration, AppSettings: "ClientApiSettings", httpSettingKey: "HttpRequestSettings");
            Helper.EmailSending.Bootstrapper.Bootstrapper.ResolveEmailSendingService(services, Configuration, settingKey: "SMTPSettings");
            Base.Resources.Bootstrapper.Bootstrapper.ResolveMessages(services);
            Notification.ApiClient.Bootstraper.Bootstrapper.ResolveMahammaNotificationClientApiService(services, Configuration, AppSettings: "ClientApiSettings", httpSettingKey: "HttpRequestSettings");
            Integration.Meeting.Zoom.Bootstrapper.Bootstrapper.ResolveMeetingZoomService(services, Configuration, AppSettings: "MeetingZoomSettings", httpSettingKey: "HttpRequestSettings");
            services.AddScoped<IWorkerServiceParallelHelper, WorkerServiceParallelHelper>();
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MahammaContext context)
        {
            #region env
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            context.Database.Migrate();
            MigrationScripts.ApplyDbScripts(Path.Combine("Migrations", "FX"), context);
            MigrationScripts.ApplyDbScripts(Path.Combine("Migrations", "SP"), context);
            Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Mahamma.Api v1"));
            #endregion

            #region CORS
            app.UseCors(AllowedOrigins);
            #endregion

            #region app
            app.UseMiddleware<JWTMiddleware>();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepareFoldersToUseStaticFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"ExcelInvitationFiles")),
                RequestPath = new PathString("/ExcelInvitationFiles")
            });
            #endregion
        }

        private void PrepareFoldersToUseStaticFiles()
        {
            string[] folderNamesList = new string[] { "ExcelInvitationFiles" };

            foreach (string folderName in folderNamesList)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        //this method gets called automatically by autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
            //container.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ApplicationModule());
        }

        private IServiceCollection AddCustomDbContext(IServiceCollection services)
        {
            string connString = Configuration["ConnectionString"];
            string envConnString = Environment.GetEnvironmentVariable("ConnectionStrings__DBConString");
            if (!string.IsNullOrWhiteSpace(envConnString))
            {
                connString = envConnString;
            }
            Log.Information($"Api will run with connection string: {connString}");

            services.AddDbContext<MahammaContext>(options =>
            {
                options.UseLazyLoadingProxies(false).UseSqlServer(connString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            }, ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );
            return services;
        }

        private IServiceCollection ActivitySettings(IServiceCollection services)
        {
            ActivitesSettings taskActivitesSettings = new();
            services.AddSingleton(taskActivitesSettings);

            return services;
        }

        private IServiceCollection JWTAppSettingsConfig(IServiceCollection services)
        {
            JWTSetting jWTSetting = new();
            Configuration.Bind("JWTSetting", jWTSetting);
            services.AddSingleton(jWTSetting);

            return services;
        }

        private IServiceCollection BackGroundServiceSettingsConfig(IServiceCollection services)
        {
            BackGroundServiceSettings backGroundServiceSettings = new();
            Configuration.Bind("BackGroundServiceSettings", backGroundServiceSettings);
            services.AddSingleton(backGroundServiceSettings);

            return services;
        }

        private IServiceCollection AppSettingsConfig(IServiceCollection services)
        {
            AppSetting appSetting = new();
            Configuration.Bind("AppSetting", appSetting);
            services.AddSingleton(appSetting);

            return services;
        }
    }
}
