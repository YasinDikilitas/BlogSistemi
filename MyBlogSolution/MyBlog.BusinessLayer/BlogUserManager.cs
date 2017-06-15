using Blog.Common.Helpers;
using MyBlog.BusinessLayer.Abstract;
using MyBlog.DataAccessLayer.EntityFramework;
using MyBlog.Entities;
using MyBlog.Entities.Messages;
using MyBlog.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BusinessLayer
{
    public class BlogUserManager : ManagerBase<BlogUser>
    {

        public BusinessLayerResult<BlogUser> RegisterUser(RegisterViewModel data) {
            BlogUser user =Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BlogUser> layerResult = new BusinessLayerResult<BlogUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExist, "Email adresi kayıtlı.");
                }
            }
            else
            {
                int dbResult = base.Insert(new BlogUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    ProfileImageFilename = "user_boy.png",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    isAdmin = false
                });

                if (dbResult > 0)
                {
                    layerResult.Result = Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerResult.Result.Email, "MyBlog Hesap Aktifleştirme");


                }

            }
            return layerResult;
        }

        public BusinessLayerResult<BlogUser> GetUserById(int id)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = Find(x => x.id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }
        public BusinessLayerResult<BlogUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<BlogUser> layerResult = new BusinessLayerResult<BlogUser>();
            layerResult.Result = Find(x => x.Username == data.Username || x.Password == data.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserIsNotActive,"Kullanıcı aktifleştirilmemiştir.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen email adresinizi kontrol ediniz.");
                }
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre hatalı");
            }
            return layerResult;
        }
        public BusinessLayerResult<BlogUser> UpdateProfile(BlogUser data)
        {
            BlogUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();

            if (db_user != null && db_user.id != data.id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.id == data.id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (Uptade(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }
        public BusinessLayerResult<BlogUser> RemoveUserById(int id)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            BlogUser user = Find(x => x.id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }
        public BusinessLayerResult<BlogUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
               Uptade(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }


        public new BusinessLayerResult<BlogUser> Insert(BlogUser data)
        {
            BlogUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user_boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<BlogUser> Update(BlogUser data)
        {
            BlogUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = data;

            if (db_user != null && db_user.id != data.id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.id == data.id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.isAdmin = data.isAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }

    }
}
