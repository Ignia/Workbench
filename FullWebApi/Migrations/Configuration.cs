namespace FullWebApi.Migrations {
  using FullWebApi.Models;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<FullWebApi.Models.ApplicationDbContext> {
    public Configuration() {
      AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(FullWebApi.Models.ApplicationDbContext context) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Create Users
      \-----------------------------------------------------------------------------------------------------------------------*/
      var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

      //Create "Jeremy" user
      var jeremy = new ApplicationUser() {
        UserName = "Jeremy",
        Email = "Jeremy@Ignia.com"
      };

      //Create "Katherine" user
      var katherine = new ApplicationUser() {
        UserName = "Katherine",
        Email = "Katie@Ignia.com"
      };

      //Establish credentials
      if (manager.Users.Count<ApplicationUser>() == 0) {
        manager.Create(jeremy, "15Password#");
        manager.Create(katherine, "15Password#");
      }

      //retrieve users
      jeremy = (ApplicationUser)manager.Users.Where<ApplicationUser>(u => u.UserName == "Jeremy").FirstOrDefault<ApplicationUser>();
      katherine = (ApplicationUser)manager.Users.Where<ApplicationUser>(u => u.UserName == "Katherine").FirstOrDefault<ApplicationUser>();

      /*------------------------------------------------------------------------------------------------------------------------
      | Create posts
      \-----------------------------------------------------------------------------------------------------------------------*/
      var post1 = new BlogPost() { Id = 1, Title = "The first post!", User = jeremy };
      var post2 = new BlogPost() { Id = 2, Title = "My first post!", User = katherine };
      var post3 = new BlogPost() { Id = 3, Title = "Another post.", User = jeremy };

      post1.User = jeremy;

      context.BlogPosts.AddOrUpdate(
        blogPost => blogPost.Id,
        post1,
        post2,
        post3
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Create comments
      \-----------------------------------------------------------------------------------------------------------------------*/
      var comment1 = new Comment { Id = 1, BlogPost = post1, Body = "Interesting!", User = jeremy };
      var comment2 = new Comment { Id = 2, BlogPost = post2, Body = "Confusing!", User = jeremy };
      var comment3 = new Comment { Id = 3, BlogPost = post3, Body = "OK.", User = katherine };
      var comment4 = new Comment { Id = 4, BlogPost = post3, Body = "Why?", User = jeremy };

      context.Comments.AddOrUpdate(
        comment => comment.Id,
        comment1,
        comment2,
        comment3,
        comment4
      );

      context.SaveChanges();

    }
  }
}
