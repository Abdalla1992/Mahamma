using Autofac;
using AutoMapper;
using Mahamma.Notification.Api.Infrastructure.AutofacHandler;
using Mahamma.Notification.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Mahamma.Notification.Infrastructure.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mahamma.Notification.Api.BackGroundService;
using Mahamma.Notification.AppService.Settings;
using Mahamma.Notification.Api.Hub;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mahamma.Notification.Api.TemporaryAppService;
using CorePush.Google;
using Mahamma.Notification.Infrastructure.MigrationSetting;
using System.IO;
using Mahamma.Notification.AppService.Notification.Helper.SendFirebaseNotification;
using Mahamma.Notification.AppService.Notification.Helper.WorkerServiceParallelHelper;

namespace Mahamma.Notification.Api
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
                mc.AddProfile(new NotificationProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Add Controllers
            services.AddControllers();
            #endregion

            #region BackGroundService
            if (bool.TryParse(Configuration["BackGroundServiceSettings:UseBackGroundService"], out bool useBackGroundService) && useBackGroundService)
                services.AddHostedService<NotificationBackGroundService>();
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
                               //.AllowCredentials()
                               //.SetIsOriginAllowed((hosts) => true);
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
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWTSetting:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWTSetting:Audience"]
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.Value.StartsWith("/notify"
                           //|| context.Request.Path.Value.StartsWith("/chat")
                           )
                            && context.Request.Query.TryGetValue("access_token", out StringValues token)
                        )
                        {
                            context.Token = token;
                            context.Request.Headers["Authorization"] = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var te = context.Exception;
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion

            #region SinalR
            services.AddHttpContextAccessor();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mahamma.Notification.Api", Version = "v1" });
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
            JWTAppSettingsConfig(services);
            BackGroundServiceSettingsConfig(services);
            FcmNotificationSettingsConfig(services);
            #endregion

            services.AddScoped<Infrastructure.Filter.AuthorizeAttribute>();

            Identity.ApiClient.Bootstrapper.ResolveMahammaIdentityClientApiService(services, Configuration, AppSettings: "ClientApiSettings", httpSettingKey: "HttpRequestSettings");

            Helper.EmailSending.Bootstrapper.Bootstrapper.ResolveEmailSendingService(services, Configuration, settingKey: "SMTPSettings");
            Base.Resources.Bootstrapper.Bootstrapper.ResolveMessages(services);


            services.AddHttpClient<FcmSender>();
            services.AddScoped<ISendSignalRNotification, SendSignalRNotification>();
            services.AddScoped<ISendFirebaseNotification, SendFirebaseNotification>();
            services.AddScoped<IWorkerServiceParallelHelper, WorkerServiceParallelHelper>();
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, NotificationContext context)
        {
            #region env
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            #endregion
            context.Database.Migrate();
            MigrationScripts.ApplyDbScripts(Path.Combine("Migrations", "FX"), context);
            MigrationScripts.ApplyDbScripts(Path.Combine("Migrations", "SP"), context);
            Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Mahamma.Notification.Api v1"));

            #region CORS
            app.UseCors(AllowedOrigins);
            #endregion

            #region app
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<Infrastructure.Middleware.JWTMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotifyHub>("/notify");
                endpoints.MapControllers();
            });
            #endregion

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

            services.AddDbContext<NotificationContext>(options =>
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

        //this method gets called automatically by autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
            //container.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ApplicationModule());
        }

        private IServiceCollection BackGroundServiceSettingsConfig(IServiceCollection services)
        {
            BackGroundServiceSettings backGroundServiceSettings = new();
            Configuration.Bind("BackGroundServiceSettings", backGroundServiceSettings);
            services.AddSingleton(backGroundServiceSettings);

            return services;
        }

        private IServiceCollection FcmNotificationSettingsConfig(IServiceCollection services)
        {
            FcmNotificationSetting fcmNotificationSetting = new();
            Configuration.Bind("FcmNotification", fcmNotificationSetting);
            services.AddSingleton(fcmNotificationSetting);

            return services;
        }

        private IServiceCollection AppSettingsConfig(IServiceCollection services)
        {
            AppSetting setting = new();
            Configuration.Bind("AppSettings", setting);
            services.AddSingleton(setting);

            return services;
        }

        private IServiceCollection JWTAppSettingsConfig(IServiceCollection services)
        {
            JWTSetting jWTSetting = new();
            Configuration.Bind("JWTSetting", jWTSetting);
            services.AddSingleton(jWTSetting);

            return services;
        }
    }
}
