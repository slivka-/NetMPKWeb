using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetMPK.WebUI.Models
{
    public class LineModel : MainViewModel
    {

        public int lineNo { get; set; }
        public string vechicle { get; set; }
        public string area { get; set; }
        public string daytime { get; set; }
        public string type { get; set; }

        public IEnumerable<LineModel> linesList { get; set; }
        public Dictionary<string, List<string>> lineRoutes { get; set; }

        public LineModel() : base()
        {

        }
    }
}