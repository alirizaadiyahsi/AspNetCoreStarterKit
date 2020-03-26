using AspNetCoreStarterKit.Domain.Entities.Authorization;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? ModifierUserId { get; set; }

        User ModifierUser { get; set; }
    }
}