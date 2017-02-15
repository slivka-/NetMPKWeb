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

        public ViewResult RouteSearch()
        {
            RoutesModel model = new RoutesModel
            {
                allStops = client.GetStopsWithStreets()
            };
            return View("Routes",model);
        }
    }
}