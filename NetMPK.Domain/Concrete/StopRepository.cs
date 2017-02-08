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

        public IEnumerable<string> StopNames
        {
            get
            {
                return client.GetStopsNames().AsEnumerable();
            }
        }

    }
}
