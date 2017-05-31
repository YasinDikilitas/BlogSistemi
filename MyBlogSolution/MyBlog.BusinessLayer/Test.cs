using MyBlog.DataAccessLayer.EntityFramework;
using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BusinessLayer
{
    public class Test
    {
        Repository<Category> repo_category = new Repository<Category>();
        Repository<BlogUser> repo_user = new Repository<BlogUser>();
        Repository<Note> repo_note = new Repository<Note>();
        Repository<Comment> repo_comment = new Repository<Comment>();
        public Test()
        {
           
            List<Category> categories = repo_category.List();
          //  List<Category> categories_filtered = repo_category.List(x=>x.id>5);

        }
        public void InsertTest()
        {
           
            int result = repo_user.Insert(new BlogUser()
            {

                Name = "aaa",
                Surname = "bbb",
                Email = "yilmaz94@hotmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                isAdmin = false,
                Username = "aaabbb",
                Password = "1111",
                CreatedOn = DateTime.Now,
                ModifyDate = DateTime.Now.AddMinutes(5),
                ModifiedUser = "YasinDikilitas"
            });
        }
        public void UptadeTest()
        {
            BlogUser user = repo_user.Find(x => x.Username == "aaabbb");
            if (user != null)
            {
                user.Username = "xxxx";
                repo_user.Save();
            }
        }

        public void DeleteTest()
        {
            BlogUser user = repo_user.Find(x => x.Username == "xxxx");
            if (user != null)
            {
                repo_user.Delete(user);
            }
        }

        public void CommentTest()
        {
            BlogUser user = repo_user.Find(x => x.id == 1);
            Note note = repo_note.Find(x => x.id == 3);
            Comment comment = new Comment()
            {
                Text = "Bu bir testtir",
                CreatedOn = DateTime.Now,
                ModifyDate = DateTime.Now,
                ModifiedUser = "YasinDikilitas",
                Note = note,
                Owner = user
            };
            repo_comment.Insert(comment);
        }
    }
}
