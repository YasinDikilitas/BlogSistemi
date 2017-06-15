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
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();


        public ActionResult Index()
        {
            return View(categoryManager.List());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = categoryManager.Find(x => x.id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }

            return View(category);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = categoryManager.Find(x => x.id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                Category cat = categoryManager.Find(x => x.id == category.id);
                cat.Title = category.Title;
                cat.Description = category.Description;

                categoryManager.Update(cat);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }
            return View(category);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = categoryManager.Find(x => x.id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.Find(x => x.id == id);
            categoryManager.Delete(category);

            CacheHelper.RemoveCategoriesFromCache();


            return RedirectToAction("Index");
        }
    }
}
