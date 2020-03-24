using System;
using AspNetCoreStarterKit.Domain.Entities.Authorization;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public class FullAuditedEntity : Entity, IFullAudited
    {
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
    }
}