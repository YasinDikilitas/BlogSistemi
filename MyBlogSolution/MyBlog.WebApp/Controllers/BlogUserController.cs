using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Entities;
using MyBlog.WebApp.Models;
using MyBlog.BusinessLayer;
using MyBlog.WebApp.Filters;

namespace MyBlog.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class BlogUserController : Controller
    {
        private BlogUserManager evernoteUserManager = new BlogUserManager();


        public ActionResult Index()
        {
            return View(evernoteUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogUser evernoteUser = evernoteUserManager.Find(x => x.id == id.Value);

            if (evernoteUser == null)
            {
                return HttpNotFound();
            }

            return View(evernoteUser);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogUser evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiyDate");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<BlogUser> res = evernoteUserManager.Insert(evernoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUser);
                }

                return RedirectToAction("Index");
            }

            return View(evernoteUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogUser evernoteUser = evernoteUserManager.Find(x => x.id == id.Value);

            if (evernoteUser == null)
            {
                return HttpNotFound();
            }

            return View(evernoteUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogUser evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiyDate");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<BlogUser> res = evernoteUserManager.Update(evernoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUser);
                }

                return RedirectToAction("Index");
            }
            return View(evernoteUser);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogUser evernoteUser = evernoteUserManager.Find(x => x.id == id.Value);

            if (evernoteUser == null)
            {
                return HttpNotFound();
            }

            return View(evernoteUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogUser evernoteUser = evernoteUserManager.Find(x => x.id == id);
            evernoteUserManager.Delete(evernoteUser);

            return RedirectToAction("Index");
        }
    }
}
