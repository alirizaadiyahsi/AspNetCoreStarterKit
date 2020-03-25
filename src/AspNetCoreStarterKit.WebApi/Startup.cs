using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;
using AspNetCoreStarterKit.EntityFramework;
using AspNetCoreStarterKit.WebApi.Infrastructure.ActionFilters;
using AspNetCoreStarterKit.WebApi.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace AspNetCoreStarterKit.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AspNetCoreStarterKitDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
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

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<UnitOfWorkActionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
