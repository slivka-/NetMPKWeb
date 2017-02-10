using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetMPK.WebUI.Controllers
{
    public class StopsController : Controller
    {
        NetMPKService.MPKServiceClient client;
        public int pageSize = 35;
        
        public StopsController()
        {
            client = new NetMPKService.MPKServiceClient("BasicHttpBinding_IMPKService");
        }

        public ViewResult StopsList(int page = 1)
        {
            var fullStopsList = client.GetStopsNames();

            Models.StopsModel model = new Models.StopsModel
            {
                isLoggedIn = Models.SessionModel.isLoggedIn,
                stopNames = fullStopsList.Skip((page - 1) * pageSize).Take(pageSize),
                pagingInfo = new Models.PagingInfo
                {
                    currentPage = page,
                    itemsPerPage = pageSize,
                    totalItems = fullStopsList.Count()
                }
            };
            return View(model);
        }

        public ViewResult StopDetails(string stopName)
        {
            var stopInfo = client.GetStopByName(stopName);
            Models.StopDetailModel model = new Models.StopDetailModel
            {
                isLoggedIn = Models.SessionModel.isLoggedIn,
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