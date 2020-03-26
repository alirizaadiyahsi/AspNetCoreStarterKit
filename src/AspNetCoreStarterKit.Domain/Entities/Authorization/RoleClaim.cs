using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}