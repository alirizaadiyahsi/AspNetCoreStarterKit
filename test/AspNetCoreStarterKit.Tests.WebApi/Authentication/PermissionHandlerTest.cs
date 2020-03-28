using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreStarterKit.Application.Authorization.Permissions;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;
using AspNetCoreStarterKit.WebApi.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetCoreStarterKit.Tests.WebApi.Authentication
{
    public class PermissionHandlerTest : ApiTestBase
    {
        [Fact]
        public async Task Should_Admin_Has_Permission()
        {
            var permissionAppService = TestServiceProvider.GetRequiredService<IPermissionAppService>();

            var requirements = new List<PermissionRequirement>
            {
                new PermissionRequirement(Permissions.Users.Create)
            };
            var authorizationHandlerContext = new AuthorizationHandlerContext(requirements, ContextAdminUser, null);
            var permissionHandler = new PermissionHandler(permissionAppService);
            await permissionHandler.HandleAsync(authorizationHandlerContext);

            Assert.True(authorizationHandlerContext.HasSucceeded);
        }

        [Fact]
        public async Task Should_Not_Member_Has_Permission()
        {
            var permissionAppService = TestServiceProvider.GetRequiredService<IPermissionAppService>();

            var requirements = new List<PermissionRequirement>
            {
                new PermissionRequirement(Permissions.Users.Create)
            };
            var authorizationHandlerContext = new AuthorizationHandlerContext(requirements, ContextMemberUser, null);
            var permissionHandler = new PermissionHandler(permissionAppService);
            await permissionHandler.HandleAsync(authorizationHandlerContext);

            Assert.False(authorizationHandlerContext.HasSucceeded);
        }
    }
}
