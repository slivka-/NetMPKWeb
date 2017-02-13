using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetMPK.WebUI.Models
{
    public class TimetableModel : MainViewModel
    {
        public string stopName { get; set; }
        public string streetName { get; set; }
        public int lineNo { get; set; }
        public string currentDirection { get; set; } 
        public IEnumerable<string> directions { get; set; }
        public IEnumerable<string> routePoints { get; set; }
        public IEnumerable<IEnumerable<string>> timeTable { get; set; }

        public TimetableModel() : base()
        {

        }
    }
}