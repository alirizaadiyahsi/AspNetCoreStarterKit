using System;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}