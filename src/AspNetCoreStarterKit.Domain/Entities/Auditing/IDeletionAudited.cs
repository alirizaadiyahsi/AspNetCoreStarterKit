using System;
using AspNetCoreStarterKit.Domain.Entities.Authorization;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface IDeletionAudited : ISoftDelete
    {
        Guid? DeleterUserId { get; set; }

        DateTime? DeletionTime { get; set; }

        User DeleterUser { get; set; }
    }
}