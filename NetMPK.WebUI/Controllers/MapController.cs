using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetMPK.WebUI.Infrastructure;
using NetMPK.WebUI.Models;
using System.Windows;
using System.Linq;
using static NetMPK.WebUI.Models.MapModel;

namespace NetMPK.WebUI.Controllers
{
    public class MapController : Controller
    {
        private NetMPKService.MPKServiceClient client;

        public MapController()
        {
            client = SessionAccess.serviceClinet;
        }
        public ViewResult MainMap()
        {
            var mapPoints = client.GetMapPoints();

            return View(new MapModel() { coords = mapPoints.Item1, clumpedStops=mapPoints.Item2, routesParts = mapPoints.Item3 });
        }
    }
}