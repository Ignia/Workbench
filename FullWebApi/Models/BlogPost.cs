using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FullWebApi.Models {
  public class BlogPost {

    public BlogPost() {
      Comments = new HashSet<Comment>();
    }

    [Required]
    [Column("BlogPostId")]
    public int Id { get; set; }

    public string Title { get; set; }

    public string UserId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

  }
}