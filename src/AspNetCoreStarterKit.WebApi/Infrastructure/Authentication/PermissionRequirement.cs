using Microsoft.AspNetCore.Authorization;

namespace WebApiBestPractices.WebApi.Core.Authentication
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