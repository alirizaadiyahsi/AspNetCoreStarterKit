using System;

namespace AspNetCoreStarterKit.Domain.Entities.Auditing
{
    public interface IHasModificationTime
    {
        DateTime? ModificationTime { get; set; }
    }
}