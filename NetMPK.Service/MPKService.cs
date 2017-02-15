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
        private MPKDSTableAdapters.Exp_Route_PointTableAdapter erpAdapter = new MPKDSTableAdapters.Exp_Route_PointTableAdapter();
        private MPKDSTableAdapters.Point_ScheduleTableAdapter psAdapter = new MPKDSTableAdapters.Point_ScheduleTableAdapter();

        #region Default
        /*
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

        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }
        */
        #endregion

        #region Stops
        public List<string> GetStopsNames()
        {
            return stopAdapter.GetData().OrderBy(s => s.Name).Select(s => s.Name).ToList();
        }

        public Dictionary<string, string> GetStopsWithStreets()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            var stops = stopAdapter.GetData().ToList();
            var streets = streetAdapter.GetData().ToList();
            foreach (var s in stops)
            {
                output.Add(
                            s.Name, 
                            streets.Where(x => x.ID.Equals(s.Street_ID))
                                   .Select(x => x.Name)
                                   .Single()
                          );
            }
            
            return output.OrderBy(x => x.Key).ToDictionary(x => x.Key, x=> x.Value);
        }

        public Tuple<int, string, string, double, double, IEnumerable<int>> GetStopByName(string stopName)
        {
            var stopInfo = stopAdapter.GetData().Where(x => x.Name.Equals(stopName)).Single();
            var streetName = streetAdapter.GetData().Where(x => x.ID.Equals(stopInfo.Street_ID)).Single();
            var lineIDs = rpAdapter.GetData().Where(x => x.Stop_ID.Equals(stopInfo.ID)).Select(x => x.Line_ID);
            var lines = lineAdapter.GetData().Where(x => lineIDs.Contains(x.ID)).Select(x => x.Line_No);

            return Tuple.Create(stopInfo.ID, stopInfo.Name, streetName.Name, stopInfo.X_Coord, stopInfo.Y_Coord, lines);
        }
        #endregion

        #region Lines
        public List<Tuple<int, string, string, string, string>> GetAllLines()
        {
            return lineAdapter.GetData().ToList().Select(x => Tuple.Create(x.Line_No, x.Vehicle, x.Area, x.Daytime, x.Type)).ToList();
        }

        public Dictionary<string, List<string>> GetLineRoutes(int lineNo)
        {
            Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();

            int lineId = lineAdapter.GetData().Where(x => x.Line_No.Equals(lineNo)).Select(x => x.ID).Single();
            List<string> directions = erpAdapter.GetData().Where(x => x.Line_No.Equals(lineNo)).Select(x => x.Direction).Distinct().ToList();
            foreach (string d in directions)
            {
                var routeStops = erpAdapter.GetData()
                                           .Where(x => x.Line_No.Equals(lineNo) && x.Direction.Equals(d))
                                           .OrderBy(x => x.Stop_No)
                                           .Select(x => x.Name)
                                           .ToList();
                                                   
                output.Add(d, routeStops);
            }
            if (output.Count == 2)
                return output;
            else
                throw new Exception();
        }

        public List<string> GetDirectionsForLine(int lineNo)
        {
            return erpAdapter.GetData()
                             .Where(x => x.Line_No.Equals(lineNo))
                             .Select(x => x.Direction)
                             .Distinct()
                             .ToList();
        }
        #endregion

        #region Streets
        public string GetStreetNameByStop(string stopName)
        {
            int streetID = stopAdapter.GetData()
                           .Where(x => x.Name.Equals(stopName))
                           .Select(x => x.Street_ID).Single();

            return streetAdapter.GetData()
                                .Where(x => x.ID.Equals(streetID))
                                .Select(x => x.Name)
                                .Single();
        }
        #endregion

        #region Timetables
        public List<List<string>> GetTimeTable(int lineNo, string stopName, string direction)
        {
            string[] weekdays = { "WEEKDAY", "SATURDAY", "HOLYDAY" };
            List<List<string>> output = new List<List<string>>();

            int routePointID = erpAdapter.GetData()
                                         .Where(x => x.Line_No.Equals(lineNo) && x.Name.Equals(stopName) && x.Direction.Equals(direction))
                                         .Select(x => x.ID)
                                         .Single();

            List<MPKDS.Point_ScheduleRow> data =  psAdapter.GetData().Where(x => x.Route_Point_ID.Equals(routePointID)).ToList();
            var weekRow = data.Where(x => x.Day_Type.Equals("WEEKDAY")).Single();
            var satRow = data.Where(x => x.Day_Type.Equals("SATURDAY")).Single();
            var holyRow = data.Where(x => x.Day_Type.Equals("HOLYDAY")).Single();
            for (int i = 0; i < 24; i++)
            {
                List<string> temp = new List<string>();
                temp.Add((weekRow["H" + i]!=null)? weekRow["H" + i].ToString():"EMPTY");
                temp.Add((satRow["H" + i] != null) ? satRow["H" + i].ToString() : "EMPTY");
                temp.Add((holyRow["H" + i] != null) ? holyRow["H" + i].ToString() : "EMPTY");
                output.Add(new List<string>(temp));
            }
            return output;
        }
        #endregion

    }
}
