using MyBlog.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.WebApp.ViewModels
{
    public class WarningViewModel : NotifyViewModelBase<ErrorMessageObj>
    {
        public WarningViewModel()
        {
            Title = "Uyarı!";
        }
    }
}