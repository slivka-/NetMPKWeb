using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetMPK.WebUI.Models;
using NetMPK.WebUI.Infrastructure;

namespace NetMPK.WebUI.Controllers
{
    public class LinesController : Controller
    {
        private NetMPKService.MPKServiceClient client;
        public LinesController()
        {
            client = SessionAccess.serviceClinet;
        }
        public ViewResult LinesList()
        {
            LineModel output = new LineModel();
            List<LineModel> linesList = new List<LineModel>();
            client.GetAllLines().ForEach(x => linesList.Add(new LineModel
            {
                lineNo = x.Item1,
                vechicle = x.Item2,
                area = x.Item3,
                daytime = x.Item4,
                type = x.Item5
            }));
            output.linesList = linesList;
            return View(output);
        }

        public ViewResult LineDetails(LineModel lineInfo)
        {
            lineInfo.lineRoutes = client.GetLineRoutes(lineInfo.lineNo);
            return View("LineDetails", lineInfo);
        }
    }
}