using System;
using System.Collections.Generic;

namespace NetMPK.WebUI.Models
{
    public class MapModel : MainViewModel
    {
        public List<Tuple<string,int,int>> coords{ get; set; }
        public MapModel() : base()
        {

        }
    }
}