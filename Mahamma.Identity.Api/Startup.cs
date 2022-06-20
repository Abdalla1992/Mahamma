using Autofac;
using Autofac.Extensions.DependencyInjection;
using Mahamma.Identity.Api.Infrastructure.AutofacHandler;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
using Mahamma.Identity.AppService.Account.Helper;
using AutoMapper;
using Mahamma.Identity.Infrastructure.AutoMapper;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Entity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Mahamma.Identity.Api.Setting;

namespace Mahamma.Identity.Api
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

            #region Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new IdentityProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Add Controllers
            services.AddControllers();
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

            #region DbContext & Settings
            AddCustomDbContext(services);
            AppSettingsConfig(services);
            JWTAppSettingsConfig(services);
            GoogleAppSettingsConfig(services);
            #endregion

            #region Identity
            services.AddScoped<IRoleValidator<Role>, MyRoleValidator>(); //this line...
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
                .AddRoleValidator<MyRoleValidator>() //this line ...
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            //services.AddIdentity<User, Role>(options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequiredLength = 8;
            //}).AddEntityFrameworkStores<IdentityContext>()
            ////.AddUserManager<CustomUserManager>()
            ////.AddUserStore<CustomUserStore>()
            //.AddDefaultTokenProviders();
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
                    ValidateAudience = false,
                    ValidAudience = Configuration["JWTSetting:Audience"]
                };
            });
            //services.AddAuthorization(options =>
            //{
            //    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            //        JwtBearerDefaults.AuthenticationScheme);

            //    defaultAuthorizationPolicyBuilder =
            //        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

            //    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            //});
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mahamma.Identity.Api", Version = "v1" });
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

            services.AddScoped<IJWTHelper, JWTHelper>();

            ApiClient.Bootstrapper.ResolveMahammaClientApiService(services, Configuration, AppSettings: "ClientApiSettings", httpSettingKey: "HttpRequestSettings");
            Helper.EmailSending.Bootstrapper.Bootstrapper.ResolveEmailSendingService(services, Configuration, settingKey: "SMTPSettings");

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
            #region env
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Mahamma.Identity.Api v1"));
            app.UseMiddleware<Infrastructure.Middleware.JWTMiddleware>();
            #endregion
            //app.UseHttpsRedirection();

            #region CORS
            app.UseCors(AllowedOrigins);
            #endregion

            #region app
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
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

            services.AddDbContext<IdentityContext>(options =>
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
        private IServiceCollection JWTAppSettingsConfig(IServiceCollection services)
        {
            JWTSetting jWTSetting = new();
            Configuration.Bind("JWTSetting", jWTSetting);
            services.AddSingleton(jWTSetting);

            return services;
        }
        private IServiceCollection AppSettingsConfig(IServiceCollection services)
        {
            AppSetting appSetting = new();
            Configuration.Bind("AppSetting", appSetting);
            services.AddSingleton(appSetting);

            return services;
        }

        private IServiceCollection GoogleAppSettingsConfig(IServiceCollection services)
        {
            GoogleAuthSettings goolgeSettings = new();
            Configuration.Bind("GoogleAuthSettings", goolgeSettings);
            services.AddSingleton(goolgeSettings);

            return services;
        }
    }
}
