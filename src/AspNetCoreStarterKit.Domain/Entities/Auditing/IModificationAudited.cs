using System;
using AspNetCoreStarterKit.Domain.Entities.Authorization;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? ModifierUserId { get; set; }

        User ModifierUser { get; set; }
    }
}