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
        private MPKDSTableAdapters.LineTableAdapter lineAdapter = new MPKDSTableAdapters.LineTableAdapter();
        private MPKDSTableAdapters.Route_PointTableAdapter rpAdapter = new MPKDSTableAdapters.Route_PointTableAdapter();
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

        public Tuple<int, string, string, double, double, IEnumerable<int>> GetStopByName(string stopName)
        {
            var stopInfo = stopAdapter.GetData().Where(x => x.Name.Equals(stopName)).Single();
            var streetName = streetAdapter.GetData().Where(x => x.ID.Equals(stopInfo.Street_ID)).Single();
            var lineIDs = rpAdapter.GetData().Where(x => x.Stop_ID.Equals(stopInfo.ID)).Select(x => x.Line_ID);
            var lines = lineAdapter.GetData().Where(x => lineIDs.Contains(x.ID)).Select(x => x.Line_No);

            return Tuple.Create(stopInfo.ID, stopInfo.Name, streetName.Name, stopInfo.X_Coord, stopInfo.Y_Coord, lines);
        }
    }
}
