using NetMPK.WebUI.Infrastructure;
using System;
using System.Collections.Generic;

namespace NetMPK.WebUI.Models
{
    public class MainViewModel
    {
        public bool isLoggedIn { get; set; }
        public string userID { get; set; }
        public string userLogin { get; set; }
        public List<Tuple<string,string>> userSavedRoutes { get; set; }

        public MainViewModel()
        {
            isLoggedIn = SessionAccess.isLoggedIn;
            userID = SessionAccess.userId;
            userLogin = SessionAccess.userLogin;
            userSavedRoutes = SessionAccess.savedRoutes;
        }
    }
}