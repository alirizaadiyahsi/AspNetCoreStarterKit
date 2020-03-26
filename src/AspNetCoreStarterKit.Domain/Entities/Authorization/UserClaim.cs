using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}