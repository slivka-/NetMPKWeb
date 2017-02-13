using System.Web.Mvc;

namespace NetMPK.WebUI.Controllers
{
    public class MapController : Controller
    {

        public ViewResult MainMap()
        {
            return View(new Models.MapModel());
        }
    }
}