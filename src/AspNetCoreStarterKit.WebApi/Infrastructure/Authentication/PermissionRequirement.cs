using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreStarterKit.WebApi.Infrastructure.Authentication
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; }
    }
}