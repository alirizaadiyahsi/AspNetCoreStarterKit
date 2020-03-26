using AspNetCoreStarterKit.Domain.Entities.Authorization;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public class AuditedEntity : Entity, IAudited
    {
        public DateTime CreationTime { get; set; }

        public Guid CreatorUserId { get; set; }

        public DateTime? ModificationTime { get; set; }

        public Guid? ModifierUserId { get; set; }

        public virtual User CreatorUser { get; set; }

        public virtual User ModifierUser { get; set; }
    }
}