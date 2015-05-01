using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Ignia.Workbench.Models {
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

  public class WorkbenchContext : IdentityDbContext<User> {

    public WorkbenchContext() : base("Workbench", throwIfV1Schema: false) {
    }

    public static WorkbenchContext Create() {
      return new WorkbenchContext();
    }

    public System.Data.Entity.DbSet<Post> Posts { get; set; }

    public System.Data.Entity.DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {

      base.OnModelCreating(modelBuilder);

      //POST COMMENTS (1:N)
      //###HACK JJC102213: Must disable cascading delete, otherwise a cyclical dependency will occur.
      //###TODO JJC102213: Determine if this prevents posts from being deleted when a user is deleted, or vice versa.
      //modelBuilder.Entity<Comment>()
      //  .HasRequired(comment => comment.Post)
      //  .WithMany(post => post.Comments)
      //  .HasForeignKey(comment => comment.PostId)
      //  .WillCascadeOnDelete(false);

      //USER'S POSTS (1:N)
      //###HACK JJC102213: Must disable cascading delete, otherwise a cyclical dependency will occur.
      //###TODO JJC102213: Determine if this prevents posts from being deleted when a user is deleted, or vice versa.
      modelBuilder.Entity<Post>()
        .HasRequired(post => post.User)
        .WithMany(user => user.Posts)
        .HasForeignKey(post => post.UserId)
        .WillCascadeOnDelete(false);

      //USER'S COMMENTS (1:N)
      //###HACK JJC102213: Must disable cascading delete, otherwise a cyclical dependency will occur.
      //###TODO JJC102213: Determine if this prevents comments from being deleted when a user is deleted, or vice versa.
      modelBuilder.Entity<Comment>()
        .HasRequired(comment => comment.User)
        .WithMany(user => user.Comments)
        .HasForeignKey(comment => comment.UserId)
        .WillCascadeOnDelete(false);

      //USER FOLLOWERS (N:N)
      modelBuilder.Entity<User>()
        .HasMany(user => user.Followers)
        .WithMany(user => user.Following)
        .Map(x => {
          x.ToTable("UserFollowing");
          x.MapLeftKey("UserID");
          x.MapRightKey("Following_UserID");
        });

      //USER'S LIKED POSTS (N:N)
      modelBuilder.Entity<User>()
        .HasMany(user => user.LikedPosts)
        .WithMany(post => post.Likes)
        .Map(x => {
          x.ToTable("PostLikes");
          x.MapLeftKey("UserId");
          x.MapRightKey("PostId");
        });

      //USER'S LIKED COMMENTS (N:N)
      modelBuilder.Entity<User>()
        .HasMany(user => user.LikedComments)
        .WithMany(comment => comment.Likes)
        .Map(x => {
          x.ToTable("CommentLikes");
          x.MapLeftKey("UserId");
          x.MapRightKey("CommentId");
        });

      //USER'S TAGGED POSTS (N:N)
      modelBuilder.Entity<Post>()
        .HasMany(post => post.TaggedUsers)
        .WithMany(user => user.TaggedInPosts)
        .Map(x => {
          x.ToTable("PostUsers");
          x.MapLeftKey("PostId");
          x.MapRightKey("UserId");
        });

    }

    public override int SaveChanges() {

      try {

        Comments.Local
          .Where(c => c.Post == null)
          .ToList()
          .ForEach(c => Comments.Remove(c));

        return base.SaveChanges();

      }

      catch (DbEntityValidationException ex) {

        var sb = new StringBuilder();

        foreach (var failure in ex.EntityValidationErrors) {
          sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
          foreach (var error in failure.ValidationErrors) {
            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
            sb.AppendLine();
          }
        }

        throw new DbEntityValidationException(
          "Entity Validation Failed - errors follow:\n" + sb.ToString(), 
          ex
        ); // Add the original exception as the innerException

      }

    }

  } //Class
} //Namespace