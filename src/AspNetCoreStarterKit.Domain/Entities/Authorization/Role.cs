using AspNetCoreStarterKit.Domain.Entities.OrganizationUnits;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AspNetCoreStarterKit.Domain.Entities.Authorization
{
    public class Role : IdentityRole<Guid>
    {
        public bool IsSystemDefault { get; set; } = false;

        public DateTime CreationTime { get; set; }

        public Guid? CreatorUserId { get; set; }

        public DateTime? ModificationTime { get; set; }

        public Guid? ModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual User CreatorUser { get; set; }

        public virtual User ModifierUser { get; set; }

        public virtual User DeleterUser { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<OrganizationUnitRole> OrganizationUnitRoles { get; set; }
    }
}