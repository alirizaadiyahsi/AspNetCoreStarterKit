using System;
using System.Collections.Generic;
using AspNetCoreStarterKit.Domain.Entities.OrganizationUnits;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName{ get; set; }

        public string Phone { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid CreatorUserId { get; set; }

        public DateTime? ModificationTime { get; set; }

        public Guid? ModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<OrganizationUnitUser> OrganizationUnitUsers { get; set; }
    }
}
