using System;
using System.Text;
using AspNetCoreStarterKit.Application.Email;
using AspNetCoreStarterKit.Application.Extensions;
using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;
using AspNetCoreStarterKit.EntityFramework;
using AspNetCoreStarterKit.WebApi.Infrastructure.ActionFilters;
using AspNetCoreStarterKit.WebApi.Infrastructure.Authentication;
using AspNetCoreStarterKit.WebApi.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

namespace AspNetCoreStarterKit.WebApi
{
    public class Startup
    {
        private static SymmetricSecurityKey _signingKey;
        private static JwtTokenConfiguration _jwtTokenConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AspNetCoreStarterKitDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(AppConfig.DefaultConnection))
                    .UseLazyLoadingProxies());

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AspNetCoreStarterKitDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                foreach (var permission in Permissions.GetAll())
                {
                    options.AddPolicy(permission,
                        policy => policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });

            services.AddControllers(setup =>
            {
                setup.Filters.AddService<UnitOfWorkActionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(AppConfig.App_CorsOriginPolicyName,
                    builder =>
                        builder.WithOrigins(AppConfig.App_CorsOrigins
                                .Split(",", StringSplitOptions.RemoveEmptyEntries))
                            .AllowAnyHeader()
                            .AllowAnyMethod());
            });

            _signingKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(AppConfig.Authentication_JwtBearer_SecurityKey));

            _jwtTokenConfiguration = new JwtTokenConfiguration
            {
                Issuer = AppConfig.Authentication_JwtBearer_Issuer,
                Audience = AppConfig.Authentication_JwtBearer_Audience,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(60),
            };

            services.Configure<JwtTokenConfiguration>(config =>
            {
                config.Audience = _jwtTokenConfiguration.Audience;
                config.EndDate = _jwtTokenConfiguration.EndDate;
                config.Issuer = _jwtTokenConfiguration.Issuer;
                config.StartDate = _jwtTokenConfiguration.StartDate;
                config.SigningCredentials = _jwtTokenConfiguration.SigningCredentials;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtTokenConfiguration.Issuer,
                    ValidAudience = _jwtTokenConfiguration.Audience,
                    IssuerSigningKey = _signingKey
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiBestPractices", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.Configure<EmailSettings>(Configuration.GetSection(AppConfig.Email_Smtp));
            services.AddSingleton<IEmailSender, EmailSender>();
            services.ConfigureApplicationService();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<UnitOfWorkActionFilter>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nucleus API V1");
            });

            app.UseCors(AppConfig.App_CorsOriginPolicyName);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
