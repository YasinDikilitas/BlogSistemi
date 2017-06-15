using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyBlog.Entities;

namespace MyBlog.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            //Adding Admin User...
            BlogUser admin = new BlogUser()
            {
                Name = "Yasin",
                Surname = "Dikilitas",
                Email = "yasin@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                isAdmin = true,
                Username = "YasinDikilitas",
                ProfileImageFilename = "user.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifyDate = DateTime.Now.AddMinutes(5),
                ModifiedUser = "YasinDikilitas"

            };
            //Adding standart User...
            BlogUser standartuser = new BlogUser()
            {
                Name = "Yilmaz",
                Surname = "Dikilitas",
                Email = "yilmaz94@hotmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                isAdmin = false,
                Username = "YilmazDikilitas",
                Password = "123456",
                ProfileImageFilename = "user.png",
                CreatedOn = DateTime.Now,
                ModifyDate = DateTime.Now.AddMinutes(5),
                ModifiedUser = "YasinDikilitas"

            };
            context.BlogUsers.Add(admin);
            context.BlogUsers.Add(standartuser);

            for (int u = 0;u < 10; u++){
                BlogUser user = new BlogUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    isAdmin = false,
                    Username = $"user{u}",
                    Password = "123456",
                    ProfileImageFilename = "user.png",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifyDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUser = $"user{u}"

                };
                context.BlogUsers.Add(user);
            }
            context.SaveChanges();
            //User list for using
            List<BlogUser> userList = context.BlogUsers.ToList();
            //Adding Fake Categories...
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    ModifiedUser = "yasindikilitas"
                };
                context.Categories.Add(cat);
                //Adding Fake Notes...
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 10); k++)
                {
                    BlogUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = "false",
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifyDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUser = owner.Username

                    };
                    cat.Notes.Add(note);

                    //Adding Fake Comments...
                    for (int c= 0; c < FakeData.NumberData.GetNumber(3,5); c++)
                    {
                        BlogUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifyDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUser = comment_owner.Username
                        };
                        note.Comments.Add(comment);
                    }

                    //Adding Fake Likes...
                   

                    for (int l = 0; l < note.LikeCount; l++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[l]
                        };
                        note.Likes.Add(liked);
                    }
                }

            }
            context.SaveChanges();
        }
    }
}
