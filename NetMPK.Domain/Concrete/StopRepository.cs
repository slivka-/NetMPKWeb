using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMPK.Domain.Abstract;
using NetMPK.Domain.Entities;

namespace NetMPK.Domain.Concrete
{
    public class StopRepository : IStopRepository
    {
        MPKService.MPKServiceClient client = new MPKService.MPKServiceClient("BasicHttpBinding_IMPKService");

        public IEnumerable<Stop> Stops
        {
            get
            {
                return client.GetStops().Select(s => new Stop { id = s.Item1,
                                                                name = s.Item2,
                                                                street = s.Item3 }).AsEnumerable();
            }
        }
    }
}
