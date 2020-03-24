using System;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}