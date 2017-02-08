using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetMPK.WebUI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string login, string password)
        {
            //cala logika odnosnie logowanie, pewnie if true = true? true:false;
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