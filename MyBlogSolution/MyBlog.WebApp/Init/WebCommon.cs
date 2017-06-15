using Blog.Common;
using MyBlog.Entities;
using MyBlog.WebApp.Models;
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
            BlogUser user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}