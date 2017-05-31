using Blog.Common;
using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                BlogUser user = HttpContext.Current.Session["login"] as BlogUser;
                return user.Username;

            }
            return "system";
        }
    }
}