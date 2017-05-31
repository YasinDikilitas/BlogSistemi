using Blog.Common.Helpers;
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
    public class BlogUserManager
    {
        private Repository<BlogUser> repo_user = new Repository<BlogUser>();

        public BusinessLayerResult<BlogUser> RegisterUser(RegisterViewModel data) {
            BlogUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.Email);
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
                int dbResult = repo_user.Insert(new BlogUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                    IsActive = false,
                    isAdmin = false

                });
                if (dbResult > 0)
                {
                    layerResult.Result = repo_user.Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerResult.Result.Email, "MyBlog Hesap Aktifleştirme");


                }

            }
            return layerResult;
        }

        public BusinessLayerResult<BlogUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<BlogUser> layerResult = new BusinessLayerResult<BlogUser>();
            layerResult.Result = repo_user.Find(x => x.Username == data.Username || x.Password == data.Password);

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

        public BusinessLayerResult<BlogUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = repo_user.Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                repo_user.Uptade(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }
    }
}
