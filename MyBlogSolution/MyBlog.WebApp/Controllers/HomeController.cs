using MyBlog.BusinessLayer;
using MyBlog.Entities;
using MyBlog.Entities.Messages;
using MyBlog.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            NoteManager nm = new NoteManager();
            return View(nm.GetAllNote().OrderByDescending(x=>x.ModifyDate).ToList()); 
        }
        
              // GET: Category
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View("Index",cat.Notes.OrderByDescending(x=>x.ModifyDate).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index",nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
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
                BlogUserManager bom = new BlogUserManager();
                BusinessLayerResult<BlogUser> result = bom.LoginUser(model);

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
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BlogUserManager bum = new BlogUserManager();
                BusinessLayerResult<BlogUser> res = bum.RegisterUser(model);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
             /*BlogUser user = null;
                try
                {
                    user = bum.RegisterUser(model);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                */
                /*     if (model.Username == "aaa")
                     {
                         ModelState.AddModelError("", "Kullanıcı adı kullanılıyor.");
                     }
                     if (model.Email == "aaa@aa.com")
                     {
                         ModelState.AddModelError("", "Email adresi kullanılıyor.");
                     }
                     foreach(var item in ModelState)
                     {
                         if (item.Value.Errors.Count > 0)
                         {
                             return View(model);
                         }
                     }*/

              /*  if (user == null)
                {
                    return View(model);
                }
                */
                return RedirectToAction("RegisterOk");
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ActivateUser(Guid id)
        {
            BlogUserManager bom = new BlogUserManager();
            BusinessLayerResult<BlogUser> res = bom.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("ActivateUserCancel");
            }
            return RedirectToAction("ActivateUserOk");
        }

        public ActionResult ActivateUserOk()
        {
            return View();
        }

        public ActionResult ActivateUserCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                 errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
    }
    }
