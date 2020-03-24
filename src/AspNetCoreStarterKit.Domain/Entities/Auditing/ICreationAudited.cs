using System;
using AspNetCoreStarterKit.Domain.Entities.Authorization;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid CreatorUserId { get; set; }

        User CreatorUser { get; set; }
    }
}