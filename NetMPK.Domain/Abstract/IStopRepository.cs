using System.Collections.Generic;
using NetMPK.Domain.Entities;

namespace NetMPK.Domain.Abstract
{
    public interface IStopRepository
    {
        IEnumerable<string> StopNames { get; }
    }
}
