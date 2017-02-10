using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetMPK.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public PartialViewResult GetPopover()
        {
            return PartialView("_LoginPopover");
        }


        public ActionResult Login(string login, string password)
        {
            //cala logika odnosnie logowanie, pewnie if true = true? true:false;
            if(login.Equals("user") && password.Equals("pass"))
                SetUserInfo();
            return RedirectToAction("Index", "Home");
        }

        private void SetUserInfo()
        {
            Models.SessionModel.isLoggedIn = true;
            //Polecam uzyc sesji i ustawic wszystkie "zmienne" odnosnie usera
        }

        private void ClearSession()
        {
            Models.SessionModel.isLoggedIn = false;
        }
    }
}