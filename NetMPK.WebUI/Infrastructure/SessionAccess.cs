using System;
using System.Web;
using System.Web.Mvc;

namespace NetMPK.WebUI.Infrastructure
{
    public static class SessionAccess
    {
        public static bool isLoggedIn
        {
            get
            {
                return HttpContext.Current.Session["isLoggedIn"] != null ? (bool)HttpContext.Current.Session["isLoggedIn"] : false;
            }
            set
            {
                    HttpContext.Current.Session["isLoggedIn"] = value;
            }
        }

        public static NetMPKService.MPKServiceClient serviceClinet
        {
            get
            {
                if (HttpContext.Current.Session["serviceClient"] == null)
                    HttpContext.Current.Session["serviceClient"] = new NetMPKService.MPKServiceClient();
                return (NetMPKService.MPKServiceClient)HttpContext.Current.Session["serviceClient"];
            }
        }
    }
}