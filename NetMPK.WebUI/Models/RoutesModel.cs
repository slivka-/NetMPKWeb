using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetMPK.WebUI.Models
{
    public class RoutesModel : MainViewModel
    {
        public Dictionary<string,string> allStops { get; set; }
        public RoutesModel() : base()
        {

        }
    }
}