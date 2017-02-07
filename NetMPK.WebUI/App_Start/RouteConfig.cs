using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NetMPK.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "StopsListRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Stops", action = "StopsList", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "LinesListRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Lines", action = "LinesList", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MainMapRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Map", action = "MainMap", id = UrlParameter.Optional }
            );
        }
    }
}
