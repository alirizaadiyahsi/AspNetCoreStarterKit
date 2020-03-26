using AspNetCoreStarterKit.Domain.Entities.Auditing;
using AspNetCoreStarterKit.Domain.Entities.Authorization;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.OrganizationUnits
{
    public class OrganizationUnitRole : CreationAuditedEntity, ISoftDelete
    {
        public Guid OrganizationUnitId { get; set; }

        public Guid RoleId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual OrganizationUnit OrganizationUnit { get; set; }

        public virtual Role Role { get; set; }
    }
}