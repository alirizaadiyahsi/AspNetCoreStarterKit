using System;
using System.Collections.Generic;
using AspNetCoreStarterKit.Domain.Entities.Auditing;
using AspNetCoreStarterKit.Domain.Entities.OrganizationUnits;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class User : IdentityUser<Guid>, IFullAudited
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

        public virtual User CreatorUser { get; set; }

        public virtual User ModifierUser { get; set; }

        public virtual User DeleterUser { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }

        public virtual ICollection<UserLogin> Logins { get; set; }

        public virtual ICollection<UserToken> Tokens { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<OrganizationUnitUser> OrganizationUnitUsers { get; set; }
    }
}
