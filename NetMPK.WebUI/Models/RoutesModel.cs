using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetMPK.WebUI.Models
{
    public class RoutesModel : MainViewModel
    {
        public bool noRoutesFound { get; set; }
        public Dictionary<string,string> allStops { get; set; }
        public List<List<Tuple<int, string, string, string, string, int>>> routes { get; set; }
        public RoutesModel() : base()
        {

        }
    }
}