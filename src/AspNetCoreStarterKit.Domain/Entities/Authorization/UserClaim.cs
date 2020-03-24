using System;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}