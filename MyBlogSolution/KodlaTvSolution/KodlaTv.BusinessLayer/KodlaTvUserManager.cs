using KodlaTv.DataAccessLayer.EntityFramework;
using KodlaTv.Entities;
using KodlaTv.Entities.Messages;
using KodlaTv.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.BusinessLayer
{
    public class KodlaTvUserManager
    {
        private Repository<KodlatvUser> repo_user = new Repository<KodlatvUser>();


        public BusinessLayerResult<KodlatvUser> RegisterUser(RegisterViewModel data)
        {
            KodlatvUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KodlatvUser> layerResult = new BusinessLayerResult<KodlatvUser>();

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
                int dbResult = repo_user.Insert(new KodlatvUser()
                {
                    Name=data.Name,
                    Surname=data.Surname,
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    ModifiedUser="system",
                    IsActive = false,
                    isAdmin = false

                });
                if (dbResult > 0)
                {
                    layerResult.Result = repo_user.Find(x => x.Email == data.Email && x.Username == data.Username);

                   /* string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerResult.Result.Email, "MyBlog Hesap Aktifleştirme");*/


                }

            }
            return layerResult;
        }


        public BusinessLayerResult<KodlatvUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<KodlatvUser> layerResult = new BusinessLayerResult<KodlatvUser>();
            layerResult.Result = repo_user.Find(x => x.Email == data.Email || x.Password == data.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen email adresinizi kontrol ediniz.");
                }
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre hatalı");
            }
            return layerResult;
        }


    }
}
