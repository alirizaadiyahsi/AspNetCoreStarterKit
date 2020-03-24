using System;
using AspNetCoreStarterKit.Domain.Entities.Auditing;
using AspNetCoreStarterKit.Domain.Entities.Authorization;

namespace AspNetCoreStarterKit.Domain.Entities.OrganizationUnits
{
    public class OrganizationUnitUser : CreationAuditedEntity, ISoftDelete
    {
        public Guid OrganizationUnitId { get; set; }

        public Guid UserId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual OrganizationUnit OrganizationUnit { get; set; }

        public virtual User User { get; set; }
    }
}