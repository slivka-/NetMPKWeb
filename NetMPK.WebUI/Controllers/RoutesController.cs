using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetMPK.WebUI.Models;
using NetMPK.WebUI.Infrastructure;

namespace NetMPK.WebUI.Controllers
{
    public class RoutesController : Controller
    {
        private NetMPKService.MPKServiceClient client;

        public RoutesController()
        {
            client = SessionAccess.serviceClinet;
        }

        public ViewResult FindRoutes(string startName, string stopName)
        {
            var tempRoutes = client.GetRoutes(startName, stopName);
            RoutesModel model = new RoutesModel
            {
                allStops = client.GetStopsWithStreets(),
                noRoutesFound = false
        };
            if (tempRoutes != null)
                model.routes = tempRoutes;
            else
                model.noRoutesFound = true;
            return View("Routes", model);
        }

        public ViewResult RouteSearch()
        {
            RoutesModel model = new RoutesModel
            {
                allStops = client.GetStopsWithStreets(),
                noRoutesFound = false
        };
            return View("Routes",model);
        }


    }
}