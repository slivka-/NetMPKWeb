using System.Collections.Generic;

namespace NetMPK.WebUI.Models
{
    public class StopsModel : MainViewModel
    {
        public IEnumerable<string> stopNames { get; set; }
        public PagingInfo pagingInfo { get; set; }
    }
}