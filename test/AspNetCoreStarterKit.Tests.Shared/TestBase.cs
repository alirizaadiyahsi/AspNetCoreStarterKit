using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using AspNetCoreStarterKit.EntityFramework;
using AspNetCoreStarterKit.EntityFramework.DataSeeder;
using AspNetCoreStarterKit.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreStarterKit.Tests.Shared
{
    public class TestBase
    {
        protected readonly AspNetCoreStarterKitDbContext DbContext;
        protected readonly IServiceProvider TestServiceProvider;

        public TestBase()
        {
            TestServiceProvider = TestServer.Host.Services;
            DbContext = TestServiceProvider.GetRequiredService<AspNetCoreStarterKitDbContext>();
            DbContext.Database.EnsureCreated();
            new DbContextDataSeeder(DbContext).SeedData();
        }

        protected IServiceProvider GetNewScopeTestServiceProvider()
        {
            return TestServiceProvider.CreateScope().ServiceProvider;
        }

        protected static ClaimsPrincipal ContextAdminUser => new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name,  DbContextDataSeeder.AdminUserName)
                },
                "TestAuthenticationType"
            )
        );

        protected static ClaimsPrincipal ContextMemberUser => new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name,  DbContextDataSeeder.MemberUserName)
                },
                "TestAuthenticationType"
            )
        );

        protected static TestServer TestServer => new TestServer(
            new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    services.AddDbContext<AspNetCoreStarterKitDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("AspNetCoreStarterKit")
                            .UseLazyLoadingProxies()
                            .EnableSensitiveDataLogging();
                    });
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.SetBasePath(Path.Combine(Path.GetFullPath(@"../../../.."), "AspNetCoreStarterKit.Tests.Shared"));
                    config.AddJsonFile("appsettings.json", false);
                })
        );
    }
}
