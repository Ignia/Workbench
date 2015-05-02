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

  /*============================================================================================================================
  | CLASS: CONFIGURATION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides configuration settingsfor Entity Framework migrations, including a method for seeding the database.
  /// </summary>
  internal sealed class Configuration : DbMigrationsConfiguration<Ignia.Workbench.Models.WorkbenchContext> {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>Initializes a new instance of the <see cref="Configuration"/> class.</summary>
    public Configuration() {
      AutomaticMigrationsEnabled = true;
      ContextKey = "Ignia.Workbench.Models.WorkbenchContext";
    }

    /*==========================================================================================================================
    | SEED METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Seeds the database anytime a migration is executed.
    /// </summary>
    /// <param name="context">The Entity Framework context.</param>
    protected override void Seed(Ignia.Workbench.Models.WorkbenchContext context) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Create Users
      \-----------------------------------------------------------------------------------------------------------------------*/
      var manager    = new UserManager<User>(new UserStore<User>(context));

      var jeremy     = CreateUser(manager, "Jeremy",    "Jeremy@Ignia.com",    "15Password#");
      var katherine  = CreateUser(manager, "Katherine", "Katherine@Ignia.com", "15Password#");

      /*------------------------------------------------------------------------------------------------------------------------
      | Add followers (friends)
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (jeremy.Followers.Count == 0) jeremy.Followers.Add(katherine);
      if (katherine.Followers.Count == 0) katherine.Followers.Add(jeremy);

      /*------------------------------------------------------------------------------------------------------------------------
      | Create posts
      \-----------------------------------------------------------------------------------------------------------------------*/
      var post1 = new Post() { Id = 1, Title = "The first post!", Body = "Content 1", UserId = jeremy.Id };
      var post2 = new Post() { Id = 2, Title = "My first post!", Body = "Content 2", UserId = katherine.Id };
      var post3 = new Post() { Id = 3, Title = "Another post.", Body = "Content 3", UserId = jeremy.Id };

      //Like posts
      post1.Likes.Add(katherine);
      post2.Likes.Add(jeremy);
      post2.Likes.Add(katherine);
      post3.Likes.Add(jeremy);

      //Tag users
      post1.TaggedUsers.Add(katherine);
      post2.TaggedUsers.Add(katherine);
      post2.TaggedUsers.Add(jeremy);

      context.Posts.AddOrUpdate(
        post => post.Id,
        post1,
        post2,
        post3
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Create comments
      \-----------------------------------------------------------------------------------------------------------------------*/
      var comment1 = new Comment { Id = 1, PostId = post1.Id, Post = post1, Body = "Interesting!", UserId = jeremy.Id, User = jeremy };
      var comment2 = new Comment { Id = 2, PostId = post2.Id, Post = post2, Body = "Confusing!", UserId = jeremy.Id, User = jeremy };
      var comment3 = new Comment { Id = 3, PostId = post3.Id, Post = post3, Body = "OK.", UserId = katherine.Id, User = katherine };
      var comment4 = new Comment { Id = 4, PostId = post3.Id, Post = post3, Body = "Why?", UserId = jeremy.Id, User = jeremy };

      //Like comments
      comment1.Likes.Add(katherine);
      comment2.Likes.Add(jeremy);
      comment2.Likes.Add(katherine);

      context.Comments.AddOrUpdate(
        comment => comment.Id,
        comment1,
        comment2,
        comment3,
        comment4
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Save changes
      \-----------------------------------------------------------------------------------------------------------------------*/
      context.SaveChanges();

    }

    /*==========================================================================================================================
    | CREATE USER METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Looks up a user and, if they don't exist, creates a user record.
    /// </summary>
    /// <param name="manager">The user manager to use to create the user.</param>
    /// <param name="username">The requested username; will also be used as the key to determine if the user exists.</param>
    /// <param name="email">The requested email.</param>
    /// <param name="password">The requested password.</param>
    /// <returns></returns>
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

    /*==========================================================================================================================
    | GET USER METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets a reference to the user from the <see cref="UserManager{TUser, TKey}"/>. If the user cannot be found, returns null.
    /// </summary>
    /// <param name="manager">The user manager to use to create the user.</param>
    /// <param name="username">The username; will be used as the key to determine if the user exists.</param>
    /// <returns></returns>
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
