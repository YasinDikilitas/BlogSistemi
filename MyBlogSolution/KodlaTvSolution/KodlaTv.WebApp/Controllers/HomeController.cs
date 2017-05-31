using KodlaTv.BusinessLayer;
using KodlaTv.Entities;
using KodlaTv.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KodlaTv.WebApp.Controllers
{
    public class HomeController : Controller
    {

        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        // GET: Home
        public ActionResult Index()
        {
           // BusinessLayer.Test test = new BusinessLayer.Test();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                BusinessLayerResult<KodlatvUser> result = kodlatvusermanager.LoginUser(model);

                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                Session["login"] = result.Result;
                return RedirectToAction("Index");

            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index")
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                KodlaTvUserManager bum = new KodlaTvUserManager();
                BusinessLayerResult<KodlatvUser> res = bum.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                return RedirectToAction("RegisterOk");
            }
                return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            return View();
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}