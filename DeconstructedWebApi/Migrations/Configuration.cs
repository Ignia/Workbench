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
      var manager    = new UserManager<User>(new UserStore<User>(context));

      var jeremy     = CreateUser(manager, "Jeremy",    "Jeremy@Ignia.com",    "15Password#");
      var katherine  = CreateUser(manager, "Katherine", "Katherine@Ignia.com", "15Password#");

      //Retrieve users
      jeremy = (User)manager.Users.Where<User>(u => u.UserName == "Jeremy").FirstOrDefault<User>();
      katherine = (User)manager.Users.Where<User>(u => u.UserName == "Katherine").FirstOrDefault<User>();

      /*------------------------------------------------------------------------------------------------------------------------
      | Add followers (friends)
      \-----------------------------------------------------------------------------------------------------------------------*/
      jeremy.Followers.Add(katherine);
      katherine.Followers.Add(jeremy);

      /*------------------------------------------------------------------------------------------------------------------------
      | Create posts
      \-----------------------------------------------------------------------------------------------------------------------*/
      var post1 = new Post() { Id = 1, Title = "The first post!", Body = "Content 1", UserId = jeremy.Id };
      var post2 = new Post() { Id = 2, Title = "My first post!", Body = "Content 2", UserId = katherine.Id };
      var post3 = new Post() { Id = 3, Title = "Another post.", Body = "Content 3", UserId = jeremy.Id };

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
      var comment1 = new Comment { Id = 1, PostId = post1.Id, Body = "Interesting!", UserId = jeremy.Id, User = jeremy };
      var comment2 = new Comment { Id = 2, PostId = post2.Id, Body = "Confusing!", UserId = jeremy.Id, User = jeremy };
      var comment3 = new Comment { Id = 3, PostId = post3.Id, Body = "OK.", UserId = katherine.Id, User = katherine };
      var comment4 = new Comment { Id = 4, PostId = post3.Id, Body = "Why?", UserId = jeremy.Id, User = jeremy };

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

      /*------------------------------------------------------------------------------------------------------------------------
      | Save changes
      \-----------------------------------------------------------------------------------------------------------------------*/
      context.SaveChanges();

    }

    private User CreateUser(UserManager<User> manager, string username, string email, string password) {

      //Check if user exists
      var user = GetUser(manager, username);

      //Create user
      if (user == null) {

        user = new User() {
          UserName = username,
          Email = email
        };

        //Establish credentials
        manager.Create(user, password);

        //Retrieve newly created user
        user = GetUser(manager, username);

      }

      //Return user
      return user;

    }

    private User GetUser(UserManager<User> manager, string username) {

      //Retrieve user
      var user = (User)manager
        .Users
        .Where<User>(u => u.UserName == username)
        .FirstOrDefault<User>();

      //Return user
      return user;
    }

  } //Class
} //Namespace
