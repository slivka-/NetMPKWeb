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
        
        private MPKDSTableAdapters.StreetTableAdapter streetAdapter;
        private MPKDSTableAdapters.StopTableAdapter stopAdapter;
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public List<Tuple<int, string, string>> GetStops()
        {
            streetAdapter = new MPKDSTableAdapters.StreetTableAdapter();
            streetAdapter.Connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["connString"];
            stopAdapter = new MPKDSTableAdapters.StopTableAdapter();
            stopAdapter.Connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["connString"];
            MPKDS.StreetDataTable streetData = streetAdapter.GetData();
            MPKDS.StopDataTable stopData = stopAdapter.GetData();
            return stopData.Select(s => Tuple.Create(s.ID, s.Name, streetData.Where(st => st.ID.Equals(s.Street_ID)).Select(st => st.Name).First())).Take(50).ToList();
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
