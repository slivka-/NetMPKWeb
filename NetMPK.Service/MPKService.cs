using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NetMPK.Service
{
    public class MPKService : IMPKService
    {
        public MPKService()
        {
            Properties.Settings.Default.NetMPKDBConn = ConfigurationManager.AppSettings["connString"];
        }

        private MPKDSTableAdapters.StreetTableAdapter streetAdapter = new MPKDSTableAdapters.StreetTableAdapter();
        private MPKDSTableAdapters.StopTableAdapter stopAdapter = new MPKDSTableAdapters.StopTableAdapter();
        private MPKDSTableAdapters.LineTableAdapter lineAdapter = new MPKDSTableAdapters.LineTableAdapter();
        private MPKDSTableAdapters.Route_PointTableAdapter rpAdapter = new MPKDSTableAdapters.Route_PointTableAdapter();
        private MPKDSTableAdapters.Exp_Route_PointTableAdapter erpAdapter = new MPKDSTableAdapters.Exp_Route_PointTableAdapter();
        private MPKDSTableAdapters.Point_ScheduleTableAdapter psAdapter = new MPKDSTableAdapters.Point_ScheduleTableAdapter();
        private MPKDSTableAdapters.QueriesTableAdapter qAdapter = new MPKDSTableAdapters.QueriesTableAdapter();

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

        #region RoutesSearch

        public List<string> GetRoute(string startName, string stopName)
        {
            List<_LineInfo> intersectLines;

            List<_LineInfo> lastStopLines = new List<_LineInfo>();
            erpAdapter.GetData()
                      .Where(x => x.Name.Equals(stopName))
                      .Select(x => x.Line_No)
                      .Distinct()
                      .ToList()
                      .ForEach(x => lastStopLines.Add(new _LineInfo { lineNo = x, parentLine = null }));

            List<_LineInfo> currentLines = new List<_LineInfo>();
            erpAdapter.GetData()
                      .Where(x => x.Name.Equals(startName))
                      .Select(x => x.Line_No)
                      .Distinct()
                      .ToList()
                      .ForEach(x => currentLines.Add(new _LineInfo { lineNo = x, parentLine = null }));

            intersectLines = lastStopLines.Intersect(currentLines, new _LineInfoEqualityComparer()).ToList();


            while (!intersectLines.Any())
            {
                List<_LineInfo> linesToAdd = new List<_LineInfo>();
                foreach (var line in currentLines)
                {
                    var lineStops = erpAdapter.GetData()
                                                      .Where(x => x.Line_No.Equals(line.lineNo))
                                                      .Select(x => x.Name)
                                                      .Distinct()
                                                      .ToList();
                    var lineCrosses = erpAdapter.GetData()
                                                .Where(x => lineStops.Contains(x.Name))
                                                .Select(x => x.Line_No)
                                                .Distinct()
                                                .ToList();
                    foreach (int crossedLineNo in lineCrosses)
                    {
                        var li = new _LineInfo { lineNo = crossedLineNo, parentLine = line };
                        if (!currentLines.Select(x => x.lineNo).Contains(li.lineNo) && !linesToAdd.Select(x => x.lineNo).Contains(li.lineNo))
                        {
                            linesToAdd.Add(li);
                        }
                    }
                }
                linesToAdd.ForEach(x => currentLines.Add((_LineInfo)x.Clone()));

                foreach (var lta in currentLines)
                {
                    if (lastStopLines.Select(x => x.lineNo).Contains(lta.lineNo))
                        intersectLines.Add((_LineInfo)lta.Clone());
                }
            }

            List<List<int>> routeLines = new List<List<int>>();
            foreach (var l in intersectLines)
            {
                var line = new List<int>();
                line.Add(l.lineNo);
                _LineInfo currentLine = l.parentLine;
                while (currentLine != null)
                {
                    line.Add(currentLine.lineNo);
                    currentLine = currentLine.parentLine;
                }
                routeLines.Add(line);
            }
            
           
            return null;
        }

        private List<string> GetNeighbouringStops(string stopName)
        {
            List<string> neighbouringStops = new List<string>();
            List <int> stopLines = erpAdapter.GetData()
                                            .Where(x => x.Name.Equals(stopName))
                                            .OrderBy(x => x.Line_No)
                                            .Select(x => x.Line_No)
                                            .Distinct()
                                            .ToList();
            foreach (int line in stopLines)
            {
                neighbouringStops.AddRange(qAdapter.GetPrevAndNextStop(stopName,line).Split('|'));
            }

            return neighbouringStops.Where(x => x!="").Distinct().ToList();
        }

        private class _LineInfo : IComparable<_LineInfo>, ICloneable
        {
            public int lineNo { get; set; }
            public _LineInfo parentLine { get; set; }

            public object Clone()
            {
                return new _LineInfo { lineNo = this.lineNo, parentLine = (this.parentLine!=null)?(_LineInfo)this.parentLine.Clone():null };
            }

            public int CompareTo(_LineInfo other)
            {
                return lineNo.CompareTo(other.lineNo);
            }
        }

        private class _LineInfoEqualityComparer : IEqualityComparer<_LineInfo>
        {
            public bool Equals(_LineInfo x, _LineInfo y)
            {
                return x.lineNo == y.lineNo;
            }

            public int GetHashCode(_LineInfo obj)
            {
                unchecked
                {
                    var hash = 17;
                    hash = hash * 23 + obj.lineNo.GetHashCode();
                    return hash;
                }
            }
        }

        #endregion
    }
}
