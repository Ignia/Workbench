namespace Ignia.Workbench.DeconstructedWebApi.Migrations {
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;
  using Ignia.Workbench.Models;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using System.Data.Entity.Validation;
  using System.Text;

  internal sealed class Configuration : DbMigrationsConfiguration<Ignia.Workbench.Models.WorkbenchContext> {
    public Configuration() {
      AutomaticMigrationsEnabled = true;
      ContextKey = "Ignia.Workbench.Models.WorkbenchContext";
    }

    protected override void Seed(Ignia.Workbench.Models.WorkbenchContext context) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Create Users
      \-----------------------------------------------------------------------------------------------------------------------*/
      var manager = new UserManager<User>(new UserStore<User>(context));

      //Create "Jeremy" user
      var jeremy = new User() {
        UserName = "Jeremy",
        Email = "Jeremy@Ignia.com"
      };

      //Create "Katherine" user
      var katherine = new User() {
        UserName = "Katherine",
        Email = "Katie@Ignia.com"
      };

      //Establish credentials
      manager.Create(jeremy, "15Password#");
      manager.Create(katherine, "15Password#");

      /*------------------------------------------------------------------------------------------------------------------------
      | Add followers (friends)
      \-----------------------------------------------------------------------------------------------------------------------*/
      jeremy.Followers.Add(katherine);
      katherine.Followers.Add(jeremy);

      /*------------------------------------------------------------------------------------------------------------------------
      | Create posts
      \-----------------------------------------------------------------------------------------------------------------------*/
      var post1 = new Post() { Id = 1, Title = "The first post!", Body = "Content 1", User = jeremy };
      var post2 = new Post() { Id = 2, Title = "My first post!", Body = "Content 2", User = katherine };
      var post3 = new Post() { Id = 3, Title = "Another post.", Body = "Content 3", User = jeremy };

      context.Posts.AddOrUpdate(
        post => post.Id,
        post1,
        post2,
        post3
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Like posts
      \-----------------------------------------------------------------------------------------------------------------------*/
      post1.Likes.Add(katherine);
      post2.Likes.Add(jeremy);
      post2.Likes.Add(katherine);
      post3.Likes.Add(jeremy);

      /*------------------------------------------------------------------------------------------------------------------------
      | Tag users
      \-----------------------------------------------------------------------------------------------------------------------*/
      post1.TaggedUsers.Add(katherine);
      post2.TaggedUsers.Add(katherine);
      post2.TaggedUsers.Add(jeremy);

      /*------------------------------------------------------------------------------------------------------------------------
      | Create comments
      \-----------------------------------------------------------------------------------------------------------------------*/
      var comment1 = new Comment { Id = 1, Post = post1, Body = "Interesting!", User = jeremy };
      var comment2 = new Comment { Id = 2, Post = post2, Body = "Confusing!", User = jeremy };
      var comment3 = new Comment { Id = 3, Post = post3, Body = "OK.", User = katherine };
      var comment4 = new Comment { Id = 4, Post = post3, Body = "Why?", User = jeremy };

      context.Comments.AddOrUpdate(
        comment => comment.Id,
        comment1,
        comment2,
        comment3,
        comment4
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Like comments
      \-----------------------------------------------------------------------------------------------------------------------*/
      comment1.Likes.Add(katherine);
      comment2.Likes.Add(jeremy);
      comment2.Likes.Add(katherine);

      context.SaveChanges();

    }

  }
}
