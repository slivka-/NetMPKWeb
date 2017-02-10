using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetMPK.WebUI.Models
{
    public class StopDetailModel:MainViewModel
    {
        public int id { get; set; }
        public string stopName { get; set; }
        public string streetName { get; set; }
        public double xCoord { get; set; }
        public double yCoord { get; set; }
        public IEnumerable<int> stopLines { get; set; }
    }
}