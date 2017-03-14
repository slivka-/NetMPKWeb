using System;
using System.Collections.Generic;
using System.Windows;

namespace NetMPK.WebUI.Models
{
    public class MapModel : MainViewModel
    {
        public Dictionary<string, Vector> coords { get; set; }
        public List<Tuple<Vector,Vector>> routesParts { get; set; }
        public Dictionary<string, List<string>> clumpedStops { get; set; }

        public MapModel() : base()
        {

        }
    }
}