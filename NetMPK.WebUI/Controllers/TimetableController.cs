using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetMPK.WebUI.Models;
using NetMPK.WebUI.Infrastructure;

namespace NetMPK.WebUI.Controllers
{
    public class TimetableController : Controller
    {
        NetMPKService.MPKServiceClient client;
        public TimetableController()
        {
            client = SessionAccess.serviceClinet;
        }
        public ViewResult Timetable(int lineNo,string stopName,string direction=null)
        {
            var dirs = client.GetDirectionsForLine(lineNo);
            string currentDirection = (direction != null) ? direction : dirs.First();
            TimetableModel model = new TimetableModel
            {
                lineNo = lineNo,
                stopName = stopName,
                streetName = client.GetStreetNameByStop(stopName),
                currentDirection = currentDirection,
                directions = dirs,
                routePoints = client.GetLineRoutes(lineNo)[currentDirection],
                timeTable = client.GetTimeTable(lineNo, stopName, currentDirection)
            };
            return View("Timetable",model);
        }
    }
}