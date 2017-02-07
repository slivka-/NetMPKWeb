using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NetMPK.Service
{
    public class MPKService : IMPKService
    {

        private MPKDSTableAdapters.StreetTableAdapter streetAdapter = new MPKDSTableAdapters.StreetTableAdapter();
        private MPKDSTableAdapters.StopTableAdapter stopAdapter = new MPKDSTableAdapters.StopTableAdapter();
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public List<string> GetStopsNames()
        {
            MPKDS.StopDataTable stopData = stopAdapter.GetData();
            return stopData.OrderBy(s => s.Name).Select(s => s.Name).ToList();
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
