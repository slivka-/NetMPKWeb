using System;
using System.Web;
using System.Web.Mvc;

namespace NetMPK.WebUI.Models
{
    public static class SessionModel
    {
        public static bool isLoggedIn {
            get
            {
                return HttpContext.Current.Session["isLoggedIn"] != null ? (bool)HttpContext.Current.Session["isLoggedIn"] : false;
            }
            set
            {
                    HttpContext.Current.Session["isLoggedIn"] = value;
            }
        }
    }
}