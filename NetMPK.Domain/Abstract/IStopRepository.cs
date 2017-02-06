using System.Collections.Generic;
using NetMPK.Domain.Entities;

namespace NetMPK.Domain.Abstract
{
    public interface IStopRepository
    {
        IEnumerable<Stop> Stops { get; }
    }
}
