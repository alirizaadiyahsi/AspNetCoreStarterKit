﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApiBestPractices.Application.Identity.Permissions;

namespace WebApiBestPractices.WebApi.Core.Authentication
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionAppService _permissionAppService;

        public PermissionHandler(IPermissionAppService permissionAppService)
        {
            _permissionAppService = permissionAppService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var hasPermission = await _permissionAppService.IsUserGrantedToPermissionAsync(context.User.Identity.Name, requirement.Permission);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
