using NetMPK.WebUI.Infrastructure;

namespace NetMPK.WebUI.Models
{
    public class MainViewModel
    {
        public bool isLoggedIn { get; set; }
        public MainViewModel()
        {
            isLoggedIn = SessionAccess.isLoggedIn;
        }
    }
}