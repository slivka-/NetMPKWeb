using System.Web.Mvc;

namespace NetMPK.WebUI.Controllers
{
    public class HomeController : Controller
    { 
        // GET: Home
        public ActionResult Index()
        {
            return View("Index", new Models.MainViewModel { isLoggedIn = Models.SessionModel.isLoggedIn });
        }
        
    }
}