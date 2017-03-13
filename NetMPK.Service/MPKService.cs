using System;
using System.Collections.Generic;
using System.Linq;
using NetMPK.Service.MPKDBTableAdapters;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Windows;

namespace NetMPK.Service
{
    public class MPKService : IMPKService
    {
        public MPKService()
        {
            
        }

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
            [SoapHeader("Authentication")]
        public List<string> GetStopsNames()
        { 
            using (var adapter = new StopsTableAdapter())
            {
                return adapter.GetDataOrderedByName().Select(s => s.Stop_Name).ToList();
            }
        }

        public Dictionary<string, string> GetStopsWithStreets()
        {
            using (var adapter = new Stops_StreetsTableAdapter())
            {
                return adapter.GetDataOrdered().ToDictionary(k => k.Stop_Name, v => v.Street_Name);
            }
        }

        public Tuple<int, string, string, double, double, List<int>> GetStopByName(string stopName)
        {
            Stops_StreetsTableAdapter stAdapter = new Stops_StreetsTableAdapter();
            Desc_Route_PointsTableAdapter drpAdapter = new Desc_Route_PointsTableAdapter();
            try
            {
                var stop = stAdapter.GetStopByName(stopName).Single();
                var lines = drpAdapter.GetDataByStop(stopName).Select(s => s.Line_No).Distinct().ToList();
                return Tuple.Create(stop.ID, stop.Stop_Name, stop.Street_Name, stop.X_Coord, stop.Y_Coord, lines);
            }
            finally
            {
                if (stAdapter != null)
                    stAdapter.Dispose();
                if (drpAdapter != null)
                    drpAdapter.Dispose();
            }
        }

        public List<Tuple<string, double, double>> GetStopWithCords()
        {
            using (var adapter = new StopsTableAdapter())
            {
                return adapter.GetData().Select(s => Tuple.Create(s.Stop_Name, s.X_Coord, s.Y_Coord)).ToList();
            }
        }
        #endregion

        #region Lines
        public List<Tuple<int, string, string, string, string>> GetAllLines()
        {
            using (var adapter = new LinesTableAdapter())
            {
                return adapter.GetData().Select(s => Tuple.Create(s.Line_No, s.Vehicle, s.Area, s.Daytime, s.Type)).ToList();
            }
        }
        
        public Dictionary<string, List<string>> GetLineRoutes(int lineNo)
        {
            using (var adapter = new Desc_Route_PointsTableAdapter())
            {
                Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();
                var lineInfo = adapter.GetDataByLine(lineNo).ToList();
                var directions = lineInfo.Select(s => s.Direction).Distinct().ToList();
                foreach (string dir in directions)
                {
                    output.Add(dir, lineInfo.Where(w => w.Direction.Equals(dir))
                                           .OrderBy(o => o.Stop_No)
                                           .Select(s => s.Stop_Name)
                                           .ToList());
                }
                return output;
            }
        }
        
        public List<string> GetDirectionsForLine(int lineNo, string stopName)
        {
            using (var adapter = new Desc_Route_PointsTableAdapter())
            {
                return adapter.GetDataByLine(lineNo).Where(w => w.Stop_Name.Equals(stopName)).Select(s => s.Direction).Distinct().ToList();
            }
        }

        #endregion

        #region Streets
        public string GetStreetNameByStop(string stopName)
        {
            using (var adapter = new Stops_StreetsTableAdapter())
            {
                return adapter.GetStopByName(stopName).Single().Street_Name;
            }
        }
        #endregion

        #region Timetables
        public List<List<string>> GetTimeTable(int lineNo, string stopName, string direction)
        {
            List<string> weekdays = new List<string>() { "WEEKDAY", "SATURDAY", "HOLYDAY" };
            Desc_Route_PointsTableAdapter drpAdapter = new Desc_Route_PointsTableAdapter();
            Stops_With_DistanceTableAdapter swdAdapter = new Stops_With_DistanceTableAdapter();
            Desc_Line_DeparturesTableAdapter dldAdapter = new Desc_Line_DeparturesTableAdapter();
            try
            {
                List<List<string>> output = new List<List<string>>();
                Dictionary<string, List<int>> carryOver = new Dictionary<string, List<int>>() { { "WEEKDAY", new List<int>() }, { "SATURDAY", new List<int>() }, { "HOLYDAY", new List<int>() } };
                int? tempStopNo = drpAdapter.GetStopNo(lineNo, direction, stopName);
                int stopNo;
                if (tempStopNo != null)
                { 
                    stopNo = (int)tempStopNo;
                }
                else
                {
                    direction = drpAdapter.GetOppositeDirection(lineNo,direction);
                    stopNo = (int)drpAdapter.GetStopNo(lineNo, direction, stopName);
                }
                int minutesFromStart = (int)swdAdapter.GetMinsFromStart(lineNo, direction, stopNo);
                var baseTimetables = dldAdapter.GetDataByLineAndDir(lineNo, direction).ToDictionary(k => k.Day_Type, v => v);
                for (int i = 0; i < 24; i++)
                {
                    List<string> ttRow = new List<string>();
                    foreach (var wd in weekdays)
                    {
                        List<int> cell = new List<int>();
                        List<int> stopMins = new List<int>();
                        if (baseTimetables[wd]["H" + i].ToString() != "")
                            stopMins = baseTimetables[wd]["H" + i].ToString().Split(',').Select(s => (int.Parse(s) + minutesFromStart)).ToList();
                        
                        stopMins.AddRange(new List<int>(carryOver[wd]));
                        carryOver[wd] = new List<int>();
                        foreach (int min in stopMins)
                        {
                            if (min < 60)
                                cell.Add(min);
                            else
                                carryOver[wd].Add(min - 60);
                        }
                        if (cell.Any())
                            ttRow.Add(string.Join(",", cell.OrderBy(o => o)));
                        else
                            ttRow.Add("EMPTY");
                       
                    }
                    output.Add(ttRow);
                }
                return output;
            }
            finally
            {
                if (drpAdapter != null)
                    drpAdapter.Dispose();
                if (swdAdapter != null)
                    swdAdapter.Dispose();
                if (dldAdapter != null)
                    dldAdapter.Dispose();
            }
            
        }
        #endregion

        #region RoutesSearch
        public List<List<Tuple<int, string, string, string, string, int>>> GetRoutes(string startName, string stopName)
        {
            //output:List(all routes)->List(single route)->tuple(part of route)[lineNo,firstStop,departureTime,lastStop,arrivalTime,delay]
            List<List<Tuple<int, string, string, string, string, int>>> output = new List<List<Tuple<int, string, string, string, string, int>>>();
            var routeLinesList = FindRouteLines(startName, stopName);
            foreach (var routeLines in routeLinesList)
            {
                List<Tuple<int, string, string, string, string, int>> singleRoute = new List<Tuple<int, string, string, string, string, int>>();
                var routeStops = GetIntersectStops(routeLines, startName, stopName);
                bool isValid = true;
                if (routeLines.Count == routeStops.Count)
                {
                    for (int i = 0; i < routeStops.Count; i++)
                    {
                        Tuple<string, string, int> partTimes;
                        if (singleRoute.Count==0)
                            partTimes = GetTimeForPart(routeLines[i], routeStops[i],DateTime.Now.Hour+":"+DateTime.Now.Minute);
                        else
                            partTimes = GetTimeForPart(routeLines[i], routeStops[i],singleRoute.Last().Item5);
                        if (partTimes != null)
                        { 
                            singleRoute.Add(Tuple.Create(routeLines[i], routeStops[i].Item1, FormatHour(partTimes.Item1), routeStops[i].Item2, FormatHour(partTimes.Item2), partTimes.Item3));
                        }
                        else
                        {
                            isValid = false; 
                            break;
                        }
                    }
                }
                if(isValid)
                    output.Add(singleRoute);
            }
            return output;
        }

        private string FormatHour(string hour)
        {
            var temp = hour.Split(':').ToList();
            if (temp.Count != 2)
                throw new ArgumentException();
            string outputHour = (int.Parse(temp[0]) < 10) ? "0" + temp[0] : temp[0];
            string outputMinute = (int.Parse(temp[1]) < 10) ? "0" + temp[1] : temp[1];
            return outputHour + ":" + outputMinute;
        }

        #region FindTimes
        private Tuple<string,string,int> GetTimeForPart(int lineNo, Tuple<string,string,string> routePart, string prevTime = null)
        {
            Stops_With_DistanceTableAdapter swdAdapter = new Stops_With_DistanceTableAdapter();
            Desc_Line_DeparturesTableAdapter dldAdapter = new Desc_Line_DeparturesTableAdapter();
            Desc_Route_PointsTableAdapter drpAdapter = new Desc_Route_PointsTableAdapter();
            try
            {
                _PartTime outputDepartureTime;
                _PartTime outputArrivalTime;
                int delay = 0;

                var departures = dldAdapter.GetDataByLineAndDir(lineNo, routePart.Item3)
                                           .Where(w => w.Day_Type.Equals(CurrentDayType()))
                                           .Single();

                _PartTime timeToCheck = (prevTime == null) ? new _PartTime() : new _PartTime(prevTime);

                int firstStopNo = (int)drpAdapter.GetStopNo(lineNo, routePart.Item3, routePart.Item1);
                int lastStopNo = (int)drpAdapter.GetStopNo(lineNo, routePart.Item3, routePart.Item2);

                int timeToFirst = (int)swdAdapter.GetMinsFromStart(lineNo, routePart.Item3, firstStopNo);
                int timeToLast = (int)swdAdapter.GetMinsFromStart(lineNo, routePart.Item3, lastStopNo);

                timeToCheck.SubtractMinutes(timeToFirst);
                string depMinutesFull = departures["H" + timeToCheck.hour].ToString();
                while (depMinutesFull == "")
                {
                    timeToCheck.HourUp();
                    depMinutesFull = departures["H" + timeToCheck.hour].ToString();
                }
                var depMinutes = depMinutesFull.Split(',').Select(s => int.Parse(s)).Where(w => w > timeToCheck.minute).ToList();
                if (!depMinutes.Any())
                {
                    timeToCheck.HourUp();
                    outputDepartureTime = new _PartTime(timeToCheck.hour + ":" + ((string)departures["H" + timeToCheck.hour]).Split(',').First());
                }
                else
                {
                    outputDepartureTime = new _PartTime(timeToCheck.hour + ":" + depMinutes.First());
                }
                outputArrivalTime = outputDepartureTime.Copy();
                outputDepartureTime.AddMinutes(timeToFirst);
                outputArrivalTime.AddMinutes(timeToLast);
                if (prevTime != null)
                {
                    delay = outputDepartureTime.GetDifferenceInMinutes(new _PartTime(prevTime));
                }
                return Tuple.Create(outputDepartureTime.ToString(), outputArrivalTime.ToString(), delay);
            }
            catch(InvalidOperationException)
            {
                return null;
            }
            finally
            {
                if (swdAdapter != null)
                    swdAdapter.Dispose();
                if (dldAdapter != null)
                    dldAdapter.Dispose();
                if (drpAdapter != null)
                    drpAdapter.Dispose();
            }
        }

        private string CurrentDayType()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "HOLYDAY";
                case DayOfWeek.Saturday:
                    return "SATURDAY";
                default:
                    return "WEEKDAY";
            }
        }

        private class _PartTime
        {
            public int hour { get; private set; }
            public int minute { get; private set; }
            public _PartTime()
            {
                hour = DateTime.Now.Hour;
                minute = DateTime.Now.Minute;
            }
            public _PartTime(string time)
            {
                var parts = time.Split(':');
                if (parts.Length == 2)
                {
                    int tempMin = -1;
                    int tempHour = -1;
                    if (int.TryParse(parts[0], out tempHour) && tempHour < 24)
                        hour = tempHour;
                    else
                        throw new ArgumentException();
                    if (int.TryParse(parts[1], out tempMin) && tempMin < 60)
                        minute = tempMin;
                    else
                        throw new ArgumentException();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            public void AddMinutes(int minutes)
            {
                int minsToAdd = minutes % 60;
                int hoursToAdd = minutes / 60;

                minute += minsToAdd;
                if (minute >= 60)
                {
                    minute -= 60;
                    hoursToAdd += 1;
                }
                hour += hoursToAdd;
                if (hour >= 24)
                    hour -= 24;

            }
            public void SubtractMinutes(int minutes)
            {
                int minsToSub = minutes % 60;
                int hoursToSub = minutes / 60;

                minute -= minsToSub;
                if (minute < 0)
                {
                    minute += 60;
                    hoursToSub += 1;
                }
                hour -= hoursToSub;
                if (hour < 0)
                    hour += 24;
            }
            public void HourUp()
            {
                hour += 1;
                if (hour >= 24)
                    hour -= 24;
            }
            public _PartTime Copy()
            {
                return new _PartTime() { hour = hour, minute = minute };
            }
            public new string ToString()
            {
                return hour + ":" + minute;
            }
            public int GetDifferenceInMinutes(_PartTime other)
            {
                return ((hour*60)+minute)- ((other.hour * 60) + other.minute);
            }
        }

        #endregion

        #region FindStops
        private List<Tuple<string, string, string>> GetIntersectStops(List<int> lines, string startName, string stopName)
        {
            using (var adapter = new GetIntersectStopsTableAdapter())
            {
                //start,stop,direction
                List<Tuple<string, string, string>> output = new List<Tuple<string, string, string>>();
                if (lines.Count == 1)
                {
                    output.Add(Tuple.Create(startName, stopName, GetDirection(lines.First(), startName, stopName)));
                }
                else
                {
                    string partStart = startName;
                    string partEnd = "";
                    for (int i = 1; i <= lines.Count - 1; i++)
                    {
                        partEnd = adapter.GetData(lines[i - 1], lines[i]).Select(s => s.Stop_Name).First();
                        output.Add(Tuple.Create(partStart, partEnd, GetDirection(lines[i - 1], partStart, partEnd)));
                        partStart = partEnd;
                    }
                    output.Add(Tuple.Create(partStart, stopName, GetDirection(lines.Last(), partStart, stopName)));
                }
                return output;
            }
        }

        private string GetDirection(int lineNo, string startName, string stopName)
        {
            using (var adapter = new Desc_Route_PointsTableAdapter())
            {
                var dataForDir = adapter.GetDataForDirection(lineNo, startName, stopName).ToList();
                if (dataForDir[0].Stop_Name.Equals(startName))
                    return dataForDir[0].Direction;
                else
                    return dataForDir[2].Direction;
            }
        }

        #endregion

        #region FindLines
        private List<List<int>> FindRouteLines(string startName, string stopName)
        {
            Desc_Route_PointsTableAdapter drpAdapter = new Desc_Route_PointsTableAdapter();
            GetIntersectLinesTableAdapter gilAdapter = new GetIntersectLinesTableAdapter();
            LinesTableAdapter lAdapter = new LinesTableAdapter();
            try
            {
                List<List<int>> output = new List<List<int>>();
                List<int> nightLines = lAdapter.GetNightLines().Select(s => s.Line_No).ToList();
                _LineInfoEqualityComparer lComparer = new _LineInfoEqualityComparer();
                List<_LineInfo> destinationLines = new List<_LineInfo>();
                drpAdapter.GetDataByStop(stopName)
                          .Select(s => s.Line_No)
                          .Distinct()
                          .ToList()
                          .ForEach(f => destinationLines.Add(new _LineInfo() {
                                                             lineNo = f,
                                                             parentLine = null }
                          ));
                List<_LineInfo> currentLines = new List<_LineInfo>();
                drpAdapter.GetDataByStop(startName)
                          .Select(s => s.Line_No)
                          .Distinct()
                          .ToList()
                          .ForEach(f => currentLines.Add(new _LineInfo()
                          {
                              lineNo = f,
                              parentLine = null
                          }
                          ));
                List<_LineInfo> intersectLines = destinationLines.Intersect(currentLines, lComparer).ToList();

                while (!intersectLines.Any())
                {
                    List<_LineInfo> linesToAdd = new List<_LineInfo>();
                    foreach (var singleLine in currentLines)
                    {
                        var singleLineIntersects = gilAdapter.GetData(singleLine.lineNo).Select(s => s.Line_No).ToList();
                        foreach (int lineNo in singleLineIntersects)
                        {
                            _LineInfo newLineInfo = new _LineInfo() { lineNo = lineNo, parentLine = (_LineInfo)singleLine.Clone() };
                            if (!linesToAdd.Contains(newLineInfo))
                                linesToAdd.Add((_LineInfo)newLineInfo.Clone());
                        }
                    }
                    foreach (var line in linesToAdd)
                    {
                        if (!currentLines.Contains(line, lComparer))
                            currentLines.Add((_LineInfo)line.Clone());
                    }
                    foreach (var cLine in currentLines)
                    {
                        if (destinationLines.Contains(cLine, lComparer))
                            intersectLines.Add((_LineInfo)cLine.Clone());
                    }
                }

                foreach (var iLine in intersectLines)
                {
                    List<int> lineOutput = new List<int>();
                    _LineInfo currentInfo = iLine;
                    while (currentInfo != null)
                    {
                        lineOutput.Add(currentInfo.lineNo);
                        currentInfo = currentInfo.parentLine;
                    }
                    output.Add(lineOutput.Reverse<int>().ToList());
                }
                if(DateTime.Now.Hour > 4 && DateTime.Now.Hour < 24)
                {
                    List<int> idxToRemove = new List<int>();
                    for(int i=0;i<output.Count;i++)
                    {
                        if (output[i].Intersect(nightLines).Any())
                            idxToRemove.Add(i);
                    }
                    foreach (int rm in idxToRemove)
                        output.RemoveAt(rm);
                }
                return output;
            }
            finally
            {
                if (drpAdapter != null)
                    drpAdapter.Dispose();
                if (gilAdapter != null)
                    gilAdapter.Dispose();
            }
        }

        private class _LineInfo : IComparable<_LineInfo>, ICloneable
        {
            public int lineNo { get; set; }
            public _LineInfo parentLine { get; set; }

            public object Clone()
            {
                return new _LineInfo { lineNo = this.lineNo, parentLine = (this.parentLine != null) ? (_LineInfo)this.parentLine.Clone() : null };
            }
            public int CompareTo(int other)
            {
                return lineNo.CompareTo(other);
            }
            public int CompareTo(_LineInfo other)
            {
                return lineNo.CompareTo(other.lineNo);
            }
        }

        private class _LineInfoEqualityComparer : IEqualityComparer<_LineInfo>
        {
            public bool Equals(_LineInfo x, int y)
            {
                return x.lineNo == y;
            }
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

        #endregion

        #region MapDrawing
        public List<Tuple<string,int,int>> GetMapPoints()
        {
            StopsTableAdapter sAdapter = new StopsTableAdapter();
            Viable_LinesTableAdapter vlAdapter = new Viable_LinesTableAdapter();
            Desc_Route_PointsTableAdapter drpAdapter = new Desc_Route_PointsTableAdapter();
            Desc_Geo_Route_PointsTableAdapter dgrpAdapter = new Desc_Geo_Route_PointsTableAdapter();
            try
            {
                int dispWidth = 1080;
                int dispHeight = 620;
                MapPointEqualityComparer mpec = new MapPointEqualityComparer();
                List<int> linesToSet = vlAdapter.GetDataByVehicle("TRAM").OrderBy(o => o.Line_No).Select(s => s.Line_No).ToList();
                var viableStops = sAdapter.GetViableStopsByType("TRAM");
                double maxX = viableStops.Select(s => s.X_Coord).Max();
                double maxY = viableStops.Select(s => s.Y_Coord).Max();
                double minX = viableStops.Select(s => s.X_Coord).Min();
                double minY = viableStops.Select(s => s.Y_Coord).Min();
                MapPointCalculator calc = new MapPointCalculator(maxX, maxY, minX, minY, dispWidth, dispHeight);
                List<MapPoint> innetOutput = new List<MapPoint>();

                foreach (int line in linesToSet)
                {
                    var lineStops = dgrpAdapter.GetRouteForLine(line).Select(s => new MapPoint() { name = s.Stop_Name, X = s.X_Coord, Y = s.Y_Coord , isValid = (s.X_Coord!=-1)?true:false}).ToList();
                    if (lineStops.Sum(s => s.isValid.GetHashCode()) != lineStops.Count)
                    {
                        var parts = GetLineParts(lineStops);
                        if (lineStops.Count != parts.Count + 1)
                            FillMissingLineParts(ref parts, ref calc);
                        foreach (var p in parts)
                        {
                            foreach (var pt in p)
                            {
                                if (!innetOutput.Contains(pt,mpec))
                                    innetOutput.Add(pt.Clone());
                            }
                        }
                    }
                    else
                    {
                        foreach (var p in lineStops)
                            if (!innetOutput.Contains(p, mpec))
                                innetOutput.Add(p);
                    }
                }
                List<Tuple<string, int, int>> output = new List<Tuple<string, int, int>>();
                foreach (var o in innetOutput)
                {
                    var temp = calc.CalcPoint(o.X, o.Y);
                    output.Add(Tuple.Create(o.name,temp.Item1,temp.Item2));
                }
                return output;
            }
            finally
            {
                if (sAdapter != null)
                    sAdapter.Dispose();
                if (vlAdapter != null)
                    vlAdapter.Dispose();
                if (drpAdapter != null)
                    drpAdapter.Dispose();
                if (dgrpAdapter != null)
                    dgrpAdapter.Dispose();
            }
        }

        private void FillMissingLineParts(ref List<List<MapPoint>> parts, ref MapPointCalculator calc)
        {
            Desc_Points_DistanceTableAdapter dpdAdapter = new Desc_Points_DistanceTableAdapter();
            StopsTableAdapter sAdapter = new StopsTableAdapter();
            try
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    List<MapPoint> part = parts[i];
                    if (part.Count > 2)
                    {
                        MapPoint lastPoint = part.First();
                        for (int j = 1; j < part.Count - 1; j++)
                        {
                            int partDistance = 0;
                            for (int g = 1; g < part.Count; g++)
                                partDistance += (int)dpdAdapter.GetDistance(part[g - 1].name, part[g].name);

                            double ratio = (double)dpdAdapter.GetDistance(part[j - 1].name, part[j].name) / (double)partDistance;
                            MapVector pointVector = new MapVector(part.Last().X - lastPoint.X , part.Last().Y - lastPoint.Y);
                            var newPos = MapVector.AddVector(new MapVector(lastPoint.X, lastPoint.Y), MapVector.ShortenVector(pointVector, ratio));
                            part[j].X = newPos.X;
                            part[j].Y = newPos.Y;
                            part[j].isValid = true;
                            //var newCords = calc.GenerateCoordsForPoint(part[j]);
                            var stopRow = sAdapter.GetDataByStopName(part[j].name).First();
                            stopRow.X_Coord = part[j].X;
                            stopRow.Y_Coord = part[j].Y;
                            sAdapter.Update(stopRow);
                            lastPoint = part[j];
                        }
                    }
                }
            }
            finally
            {
                if (dpdAdapter != null)
                    dpdAdapter.Dispose();
                if (sAdapter != null)
                    sAdapter.Dispose();
            }
        }

        private List<List<MapPoint>> GetLineParts(List<MapPoint> line)
        {
            List<List<MapPoint>> output = new List<List<MapPoint>>();
            List<MapPoint> temp = null;
            foreach (var point in line)
            {
                if (point.isValid)
                {
                    if (temp != null)
                    {
                        temp.Add(point);
                        output.Add(temp);
                    }
                    temp = new List<MapPoint>();
                }
                temp.Add(point.Clone());
            }
            return output;
        }

        private class MapPoint
        {
            public string name { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public bool isValid { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                MapPoint p = obj as MapPoint;
                if (p == null)
                    return false;
                return name.Equals(p.name);
            }

            public bool Equals(MapPoint p)
            {
                return name.Equals(p.name);
            }

            public override int GetHashCode()
            {
                return (int)(name.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode());
            }

            public MapPoint Clone()
            {
                return new MapPoint() { name = name, X = X, Y = Y, isValid = isValid };
            }
        }

        private class MapPointEqualityComparer : IEqualityComparer<MapPoint>
        {
            public bool Equals(MapPoint x, MapPoint y)
            {
                return x.name.Equals(y.name);
            }

            public int GetHashCode(MapPoint obj)
            {
                return obj.name.GetHashCode();
            }
        }

        private class MapPointCalculator
        {
            private double xSpan;
            private double ySpan;
            private double minXCoord;
            private double minYCoord;

            private int dispWidth;
            private int dispHeight;

            public MapPointCalculator(double maxX, double maxY, double minX, double minY, int dWidth, int dHeight)
            {
                xSpan = maxX - minX;
                ySpan = maxY - minY;
                minXCoord = minX;
                minYCoord = minY;

                dispWidth = dWidth;
                dispHeight = dHeight;
            }

            public Tuple<int, int> CalcPoint(double _X, double _Y)
            {
                if (_X != -1 && _Y != -1)
                {
                    int prcX = (int)Math.Floor(((_X - minXCoord) / xSpan) * dispWidth);
                    int prcY = (int)Math.Floor(((((_Y - minYCoord) / ySpan)-1)*-1) * dispHeight);
                    return Tuple.Create(prcX, prcY);
                }
                else
                {
                    return null;
                }
            }
            /*
            public Tuple<double, double> GenerateCoordsForPoint(MapPoint point)
            {
                double crdX = (((double)point.X / dispWidth) * xSpan) + minXCoord;
                double crdY = (((double)point.Y / dispHeight) * ySpan) + minYCoord;

                return Tuple.Create(crdX, crdY);
            }
            */

        }

        private class MapVector
        {
            public double X { get; set; }
            public double Y { get; set; }

            public MapVector(double x, double y)
            {
                X = x;
                Y = y;
            }

            public static MapVector AddVector(MapVector v1, MapVector v2)
            {
                return new MapVector(v1.X + v2.X, v1.Y + v2.Y);
            }

            public static MapVector ShortenVector(MapVector v, double s)
            {
                if (v.X != 0 || v.Y != 0)
                {
                    double vLength = Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
                    double sinA = v.Y / vLength;
                    double newY = sinA * (vLength * s);
                    double newX = Math.Sqrt(Math.Pow((vLength * s), 2) - Math.Pow(newY, 2));

                    bool x = Math.Sqrt(Math.Pow(newX, 2) + Math.Pow(newY, 2)) == (vLength * s);

                    return new MapVector(newX, newY);
                }
                else
                {
                    return v;
                }
            }
        }

        #endregion

    }
}
