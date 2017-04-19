using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetMPK.WebUI.Infrastructure;

namespace NetMPK.WebUI.Controllers
{
    public class StopsController : Controller
    {
        NetMPKService.MPKServiceClient client;
        private readonly int pageSize = 120;
        
        public StopsController()
        {
            client = SessionAccess.serviceClinet;
        }

        public ViewResult StopsList(int page = 1)
        {
            var fullStopsList = client.GetStopsNames();

            Models.StopsModel model = new Models.StopsModel
            {
                stopNames = fullStopsList
            };
            return View(model);
        }

        public ViewResult StopDetails(string stopName)
        {
            var stopInfo = client.GetStopByName(stopName);
            Models.StopDetailModel model = new Models.StopDetailModel
            {
                id = stopInfo.Item1,
                stopName = stopInfo.Item2,
                streetName = stopInfo.Item3,
                xCoord = stopInfo.Item4,
                yCoord = stopInfo.Item5,
                stopLines = stopInfo.Item6
            };
            return View("StopDetail",model);
        }
    }
}