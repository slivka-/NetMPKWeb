using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
            Properties.Settings.Default.NetMPKDBConnectionString = ConfigurationManager.AppSettings["connString"];
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

            return output.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
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

            List<MPKDS.Point_ScheduleRow> data = psAdapter.GetData().Where(x => x.Route_Point_ID.Equals(routePointID)).ToList();
            var weekRow = data.Where(x => x.Day_Type.Equals("WEEKDAY")).Single();
            var satRow = data.Where(x => x.Day_Type.Equals("SATURDAY")).Single();
            var holyRow = data.Where(x => x.Day_Type.Equals("HOLYDAY")).Single();
            for (int i = 0; i < 24; i++)
            {
                List<string> temp = new List<string>();
                temp.Add((weekRow["H" + i] != null) ? weekRow["H" + i].ToString() : "EMPTY");
                temp.Add((satRow["H" + i] != null) ? satRow["H" + i].ToString() : "EMPTY");
                temp.Add((holyRow["H" + i] != null) ? holyRow["H" + i].ToString() : "EMPTY");
                output.Add(new List<string>(temp));
            }
            return output;
        }
        #endregion

        #region RoutesSearch

        public List<List<Tuple<int, string, string, string, string, int>>> GetRoutes(string startName, string stopName)
        {
            List<List<Tuple<int, string, string, string, string, int>>> functOutput = new List<List<Tuple<int, string, string, string, string, int>>>();

            List<int> nighttimeLines = lineAdapter.GetData().Where(x => x.Daytime.Equals("NIGHT"))
                                                            .Select(x => x.Line_No)
                                                            .ToList();

            List<_FullRoute> allPossibleRoutes = GetPossibleRoutes(startName, stopName).Where(x => x.allStops.First().Equals(stopName) && x.allStops.Last().Equals(startName))
                                                                                       .ToList();
            int averageStops = (int)Math.Floor(allPossibleRoutes.Select(x => x.numOfStops).Average());

            List<_FullRoute> routesToFilter = allPossibleRoutes.OrderBy(x => x.numOfLines)
                                                  .ThenBy(x => x.numOfStops)
                                                  .TakeWhile(x => x.numOfStops < averageStops)
                                                  .ToList();
            if (DateTime.Now.Hour > 4 && DateTime.Now.Hour < 23)
            {
                routesToFilter = routesToFilter.Where(x => !x.routeLines.Intersect(nighttimeLines).Any()).ToList();
            }
            List<List<_FullRoute>> pathGroups = new List<List<_FullRoute>>();
            pathGroups.Add(new List<_FullRoute>());
            pathGroups[0].Add(routesToFilter[0]);
            List<string> lastPath = routesToFilter[0].allStops;
            int currentGroup = 0;
            for (int i = 1; i < routesToFilter.Count; i++)
            {
                if (!CheckPathsEqual(lastPath, routesToFilter[i].allStops))
                {
                    currentGroup++;
                    lastPath = routesToFilter[i].allStops;
                    pathGroups.Add(new List<_FullRoute>());
                }
                pathGroups[currentGroup].Add(routesToFilter[i]);
            }
            foreach (var group in pathGroups)
            {
                                            //LineNo, start,startTime,stop,stopTime,minFromPrev
                var outputRoute = new List<Tuple<int, string, string, string, string, int>>();
                _FullRoute routeToCheck;
                bool routeBeggining = true;
                if (group.Count == 1)
                    routeToCheck = group.First();
                else
                    routeToCheck = GetMostTramRoute(group);

                foreach(var part in routeToCheck.routeParts)
                {
                    string partStart = "";
                    string partStop = "";
                    int curHour = 0;
                    int curMin = 0;
                    foreach (var stop in part.routeStops)
                    {
                        int stopId = erpAdapter.GetData()
                                               .Where(x => x.Line_No.Equals(part.lineNo) && x.Name.Equals(stop) && x.Direction.Equals(part.direction))
                                               .Select(x => x.ID)
                                               .Single();
                        var timeTable = psAdapter.GetData()
                                                 .Where(x => x.Route_Point_ID.Equals(stopId) && x.Day_Type.Equals(GetCurrentDayType()))
                                                 .Single();
                        if(stop.Equals(part.routeStops.First()))
                        {
                            #region getBegginingTime
                            int hourToCheck;
                            int minToCheck;

                            if (routeBeggining)
                            {
                                hourToCheck = DateTime.Now.Hour;
                                minToCheck = DateTime.Now.Minute;
                                routeBeggining = false;
                            }
                            else
                            {
                                hourToCheck = int.Parse(outputRoute.Last().Item5.Split(':')[0]);
                                minToCheck = int.Parse(outputRoute.Last().Item5.Split(':')[1]);
                            }
                            var mins = timeTable["H" + hourToCheck].ToString().Split(',').ToList();
                            while (mins[0] == "")
                            {
                                mins = timeTable["H" + ++hourToCheck].ToString().Split(',').ToList();
                            }
                            var startMins = mins.Where(x => int.Parse(x) >= minToCheck).ToList();
                            if (startMins.Count == 0)
                                startMins = timeTable["H" + (hourToCheck + 1)].ToString().Split(',').ToList();

                            curHour = hourToCheck;
                            curMin = int.Parse(startMins.First());
                            partStart = hourToCheck + ":" + startMins.First();
                            #endregion
                        }
                        else
                        {
                            #region calculateTimeDiff
                            var mins = timeTable["H" + curHour.ToString()].ToString().Split(',').ToList();
                            if (mins[0]!="" && mins.Where(x => int.Parse(x) >= curMin).ToList().Any())
                            { 
                                curMin = int.Parse(mins.Where(x => int.Parse(x) >= curMin).First());
                            }
                            else
                            {
                                curHour++;
                                mins = timeTable["H" + curHour.ToString()].ToString().Split(',').ToList();
                                curMin = int.Parse(mins.First());
                            }
                            #endregion
                            #region getEndTime
                            if (stop.Equals(part.routeStops.Last()))
                            {
                                partStop = curHour + ":" + curMin;
                            }
                            #endregion
                        }
                    }
                    outputRoute.Add(Tuple.Create(part.lineNo,part.routeStops.First(), partStart, part.routeStops.Last(), partStop, 0));
                }
                functOutput.Add(outputRoute);
            }
            return functOutput;
        }

        private string GetCurrentDayType()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return "HOLYDAY";
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                return "SATURDAY";
            }
            else
            {
                return "WEEKDAY";
            }
        }

        private _FullRoute GetMostTramRoute(List<_FullRoute> routes)
        {
            _FullRoute output = routes.First();
            int highestTrams = output.routeLines.Where(x => x < 100).Count();
            foreach (var r in routes)
            {
                if (r.routeLines.Where(x => x < 100).Count() > highestTrams)
                {
                    highestTrams = r.routeLines.Where(x => x < 100).Count();
                    output = r;
                }
            }
            return output;
        }

        private bool CheckPathsEqual(List<string> p1, List<string> p2)
        {
            if (p1.Count != p2.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < p1.Count; i++)
                    if (p1[i] != p2[i])
                        return false;
            }
            return true;
        }

        private List<_FullRoute> GetPossibleRoutes(string startName, string stopName)
        {
            List<_FullRoute> allPossibleRoutes = new List<_FullRoute>();
            List<_LineInfo> intersectLines;
            List<_LineInfo> lastStopLines = new List<_LineInfo>();
            List<_LineInfo> currentLines = new List<_LineInfo>();

            //Get lines arriving at last stop
            erpAdapter.GetData()
                      .Where(x => x.Name.Equals(stopName))
                      .Select(x => x.Line_No)
                      .Distinct()
                      .ToList()
                      .ForEach(x => lastStopLines.Add(new _LineInfo { lineNo = x, parentLine = null }));

            

            //Get lines arriving as first stop
            erpAdapter.GetData()
                      .Where(x => x.Name.Equals(startName))
                      .Select(x => x.Line_No)
                      .Distinct()
                      .ToList()
                      .ForEach(x => currentLines.Add(new _LineInfo { lineNo = x, parentLine = null }));
            //Check if there is a direct connection
            intersectLines = lastStopLines.Intersect(currentLines, new _LineInfoEqualityComparer()).ToList();


            while (!intersectLines.Any())
            {
                List<_LineInfo> linesToAdd = new List<_LineInfo>();
                foreach (var line in currentLines)
                {
                    //Get all stops for line
                    var lineStops = erpAdapter.GetData()
                                              .Where(x => x.Line_No.Equals(line.lineNo))
                                              .Select(x => x.Name)
                                              .Distinct()
                                              .ToList();
                    //Get all lines that arrive on stops
                    var lineCrosses = erpAdapter.GetData()
                                                .Where(x => lineStops.Contains(x.Name))
                                                .Select(x => x.Line_No)
                                                .Distinct()
                                                .ToList();
                    foreach (int crossedLineNo in lineCrosses)
                    {
                        var tempLineInfo = new _LineInfo { lineNo = crossedLineNo, parentLine = line };
                        //if line is not in list of lines to check, add it
                        if (!currentLines.Select(x => x.lineNo).Contains(tempLineInfo.lineNo) && !linesToAdd.Select(x => x.lineNo).Contains(tempLineInfo.lineNo))
                        {
                            linesToAdd.Add(tempLineInfo);
                        }
                    }
                }
                linesToAdd.ForEach(x => currentLines.Add((_LineInfo)x.Clone()));

                foreach (var lta in currentLines)
                {
                    //if line connects, add it to list
                    if (lastStopLines.Select(x => x.lineNo).Contains(lta.lineNo))
                        intersectLines.Add((_LineInfo)lta.Clone());
                }
            }

            //Make a list of all possible routes
            foreach (var singleRouteLines in intersectLines)
            {
                string partStart = stopName;//starting from the end
                string partStop = "";
                _FullRoute route = new _FullRoute();
                _LineInfo currentLine = singleRouteLines;
                while (currentLine != null)
                {
                    _RoutePart routePart = new _RoutePart();
                    routePart.lineNo = currentLine.lineNo;
                    if (currentLine.parentLine == null)//get last stop of current part
                    { 
                        partStop = startName;
                    }
                    else
                    {
                        partStop = (string)qAdapter.GetIntersectingStop(currentLine.lineNo, currentLine.parentLine.lineNo);
                    }
                    var tempPointTab = erpAdapter.GetData()
                                                 .Where(x => x.Line_No.Equals(currentLine.lineNo) && new List<string>(){partStart, partStop}.Contains(x.Name))
                                                 .OrderBy(x => x.Direction)
                                                 .ThenBy(x => x.Stop_No)
                                                 .ToList();
                    if (tempPointTab.Count == 4)//find witch direction should be taken
                    {
                        if (tempPointTab[0].Name.Equals(partStart) && tempPointTab[1].Name.Equals(partStop))
                            routePart.direction = tempPointTab[0].Direction;
                        else
                            routePart.direction = tempPointTab[2].Direction;
                    }
                    if (routePart.direction != null)
                    {
                        routePart.routeStops.AddRange(erpAdapter.GetData()
                                                                .Where(x => x.Line_No.Equals(routePart.lineNo) && x.Direction.Equals(routePart.direction))
                                                                .OrderBy(x => x.Stop_No)
                                                                .SkipWhile(x => !x.Name.Equals(partStart))
                                                                .TakeWhile(x => !x.Name.Equals(partStop))
                                                                .Select(x => x.Name)
                                                                .ToList());
                        routePart.routeStops.Add(partStop);
                        routePart.routeStops.Reverse();
                        routePart.direction = erpAdapter.GetData()
                                                        .Where(x => x.Line_No.Equals(routePart.lineNo) && !x.Direction.Equals(routePart.direction))
                                                        .Select(x => x.Direction)
                                                        .Distinct()
                                                        .Single();
                        
                    }
                    route.AddRoutePart(routePart);
                    partStart = partStop;
                    currentLine = currentLine.parentLine;
                }
                route.routeLines.Reverse();
                route.routeParts.Reverse();
                allPossibleRoutes.Add(route);   
            }
            return allPossibleRoutes;
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

        private class _FullRoute
        {
            public int numOfLines { get; private set; }
            public int numOfStops { get; private set; }
            public List<int> routeLines { get; private set; }
            public List<_RoutePart> routeParts { get; private set; }
            public List<string> allStops { get; private set; }

            public _FullRoute()
            {
                numOfLines = 0;
                numOfStops = 0;
                routeLines = new List<int>();
                routeParts = new List<_RoutePart>();
                allStops = new List<string>();
            }

            public void AddRoutePart(_RoutePart part)
            {
                numOfLines++;
                routeLines.Add(part.lineNo);
                numOfStops += part.routeStops.Count;
                routeParts.Add((_RoutePart)part.Clone());
                allStops.AddRange(part.routeStops.Reverse<string>());
            }

        }

        private class _RoutePart : ICloneable
        {
            public int lineNo { get; set; }
            public string direction { get; set; }
            public List<string> routeStops { get; set; } = new List<string>();

            public object Clone()
            {
                return new _RoutePart { lineNo = this.lineNo, direction = this.direction, routeStops = new List<string>(this.routeStops) };
            }
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
