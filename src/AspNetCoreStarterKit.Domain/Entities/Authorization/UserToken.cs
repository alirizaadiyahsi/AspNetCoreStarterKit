using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; }
    }
}