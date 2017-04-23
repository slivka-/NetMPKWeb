using System.Web.Mvc;
using NetMPK.WebUI.Infrastructure;
using NetMPK.WebUI.Models;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text;

namespace NetMPK.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private NetMPKService.MPKServiceClient client;

        public AccountController()
        {
            client = SessionAccess.serviceClinet;
        }

        public PartialViewResult GetPopover()
        {
            return PartialView("_LoginPopover");
        }

        public ViewResult DisplayRegister()
        {
            return View("Register", new RegisterModel());
        }

        public ActionResult Register(RegisterModel model)
        {
            if(model.Login != null)
                if (!client.LoginFree(model.Login))
                    ModelState.AddModelError("", "Ten login jest już zajęty");
            if(model.Email != null)
                if (!client.EmailFree(model.Email))
                    ModelState.AddModelError("", "Istnieje już konto przypisane do tego adresu email");
            if (ModelState.Values.SelectMany(s => s.Errors).Count() == 0)
            {
                if (client.RegisterUser(model.Login, model.Password.Trim(), model.Email))
                {
                    return View(new RegisterModel() {Login = null, Email = null, registerMessage = true });
                }
                else
                {
                    ModelState.AddModelError("", "Nastąpił błąd, spróbuj ponownie później");
                }
            }
            return View(model);
        }


        public ActionResult Login(string login, string password)
        {
            var loginResult = client.LoginUser(login, password.Trim());
            if(loginResult.Item1 && loginResult.Item2!=null)
                SetUserInfo(loginResult.Item2, login);
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        private void SetUserInfo(string userID,string userLogin)
        {
            SessionAccess.isLoggedIn = true;
            SessionAccess.userId = userID;
            SessionAccess.userLogin = userLogin;
        }

        public ActionResult Logout()
        {
            SessionAccess.ClearSession();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DisplayInfo()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}