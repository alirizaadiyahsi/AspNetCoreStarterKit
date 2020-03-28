using System.Threading.Tasks;
using AspNetCoreStarterKit.Application.Authorization.Permissions;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;
using AspNetCoreStarterKit.EntityFramework;
using AspNetCoreStarterKit.EntityFramework.DataSeeder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetCoreStarterKit.Tests.Application.Authorization
{
    public class PermissionAppServiceTests : AppServiceTestBase
    {
        private readonly IPermissionAppService _permissionAppService;

        public PermissionAppServiceTests()
        {
            var serviceProvider = GetServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<AspNetCoreStarterKitDbContext>();
            new DbContextDataSeeder(dbContext).SeedData();
            _permissionAppService = serviceProvider.GetRequiredService<IPermissionAppService>();
        }

        [Fact]
        public async Task Should_Permission_Granted_To_User()
        {
            var isPermissionGranted =
                await _permissionAppService.IsUserGrantedToPermissionAsync(ContextAdminUser.Identity.Name, Permissions.Users.Create);

            Assert.True(isPermissionGranted);
        }

        [Fact]
        public async Task Should_Not_Permission_Granted_To_User()
        {
            var isPermissionNotGranted =
                await _permissionAppService.IsUserGrantedToPermissionAsync(ContextMemberUser.Identity.Name, Permissions.Users.Create);

            Assert.False(isPermissionNotGranted);
        }
    }
}
