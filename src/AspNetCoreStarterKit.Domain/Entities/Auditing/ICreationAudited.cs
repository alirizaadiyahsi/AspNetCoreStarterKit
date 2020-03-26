using AspNetCoreStarterKit.Domain.Entities.Authorization;
using System;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid CreatorUserId { get; set; }

        User CreatorUser { get; set; }
    }
}