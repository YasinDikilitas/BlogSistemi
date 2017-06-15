using MyBlog.BusinessLayer;
using MyBlog.Entities;
using MyBlog.Entities.Messages;
using MyBlog.Entities.ValueObjects;
using MyBlog.WebApp.Filters;
using MyBlog.WebApp.Models;
using MyBlog.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        BlogUserManager blogusermanager = new BlogUserManager();
        NoteManager noteManager = new NoteManager();
        // GET: Home
        public ActionResult Index()
        {
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == "false").OrderByDescending(x => x.ModifyDate).ToList());
        }
        
              // GET: Category
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Note> notes = noteManager.ListQueryable().Where(
                x => x.IsDraft == "false" && x.CategoryId == id).OrderByDescending(
                x => x.ModifyDate).ToList();

            return View("Index", notes);
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
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
                BusinessLayerResult<BlogUser> res = blogusermanager.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }


                //EvernoteUser user = null;

                //try
                //{
                //    user = eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message);   
                //}



                //if (model.Username == "aaa")
                //{
                //    ModelState.AddModelError("", "Kullanıcı adı kullanılıyor.");
                //}

                //if(model.EMail == "aaa@aa.com")
                //{
                //    ModelState.AddModelError("", "E-posta adresi kullanılıyor.");
                //}

                //foreach (var item in ModelState)
                //{
                //    if(item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}

                //if (user == null)
                //{
                //    return View(model);
                //}

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };

                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");

                return View("Ok", notifyObj);
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
            BusinessLayerResult<BlogUser> res = blogusermanager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");

            return View("Ok", okNotifyObj);
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

        [Auth]
        public ActionResult ShowProfile()
        {
            BusinessLayerResult<BlogUser> res =
                blogusermanager.GetUserById(CurrentSession.User.id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            BusinessLayerResult<BlogUser> res = blogusermanager.GetUserById(CurrentSession.User.id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
           [Auth]
        [HttpPost]
        public ActionResult EditProfile(BlogUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                BusinessLayerResult<BlogUser> res = blogusermanager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<BlogUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            BusinessLayerResult<BlogUser> res =
                blogusermanager.RemoveUserById(CurrentSession.User.id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }


        public ActionResult HasError()
        {
            return View();
        }


    }
}
