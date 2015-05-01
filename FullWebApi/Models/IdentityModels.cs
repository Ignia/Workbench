using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace FullWebApi.Models {
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser {

    public ApplicationUser() : base() {
      BlogPosts = new HashSet<BlogPost>();
    }

    public ICollection<BlogPost> BlogPosts { get; set; }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
      // Add custom user claims here
      return userIdentity;
    }
  }

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false) {
    }

    public System.Data.Entity.DbSet<BlogPost> BlogPosts { get; set; }

    public static ApplicationDbContext Create() {
      return new ApplicationDbContext();
    }
    public override int SaveChanges() {
      try {
        Comments.Local
          .Where(c => c.BlogPost == null)
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
            "Entity Validation Failed - errors follow:\n" +
            sb.ToString(), ex
            ); // Add the original exception as the innerException
      }
    }

    public System.Data.Entity.DbSet<FullWebApi.Models.Comment> Comments { get; set; }
  }
}