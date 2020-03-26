using AspNetCoreStarterKit.Domain.Entities.Authorization;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public class CreationAuditedEntity : Entity, ICreationAudited
    {
        public DateTime CreationTime { get; set; }

        public Guid CreatorUserId { get; set; }

        public virtual User CreatorUser { get; set; }
    }
}