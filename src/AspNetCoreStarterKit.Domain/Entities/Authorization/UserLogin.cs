using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}