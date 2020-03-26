using AspNetCoreStarterKit.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace AspNetCoreStarterKit.Domain.Entities.OrganizationUnits
{
    public class OrganizationUnit : FullAuditedEntity
    {
        public Guid? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public virtual OrganizationUnit Parent { get; set; }

        public virtual ICollection<OrganizationUnit> Children { get; set; }

        public virtual ICollection<OrganizationUnitUser> OrganizationUnitUsers { get; set; }

        public virtual ICollection<OrganizationUnitRole> OrganizationUnitRoles { get; set; }
    }
}