using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetMPK.WebUI.Infrastructure;
using NetMPK.WebUI.Models;

namespace NetMPK.WebUI.Controllers
{
    public class MapController : Controller
    {
        private NetMPKService.MPKServiceClient client;
        private readonly double xCoordStart = 19.880878;
        private readonly double yCoordStart = 50.035669;
        private readonly double xCoordSpan = 0.22316;
        private readonly double yCoordSpan = 0.071395;
        public MapController()
        {
            client = SessionAccess.serviceClinet;
        }
        public ViewResult MainMap()
        {
            return View(new MapModel() { coords = client.GetMapPoints() });
        }
    }
}