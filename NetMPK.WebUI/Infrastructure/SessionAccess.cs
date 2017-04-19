using System;
using System.Collections.Generic;
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

        public static string userId
        {
            get
            {
                return HttpContext.Current.Session["userId"] != null ? (string)HttpContext.Current.Session["userId"] : null;
            }
            set
            {
                HttpContext.Current.Session["userId"] = value;
            }
        }

        public static string userLogin
        {
            get
            {
                return HttpContext.Current.Session["userLogin"] != null ? (string)HttpContext.Current.Session["userLogin"] : null;
            }
            set
            {
                HttpContext.Current.Session["userLogin"] = value;
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

        public static List<Tuple<string, string>> savedRoutes
        {
            get
            {
                if (isLoggedIn && userId != null)
                    return serviceClinet.GetSavedRoutesForUser(int.Parse(userId));
                else
                    return null;
            }
        }


        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}