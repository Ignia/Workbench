using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FullWebApi.Models {

  public class Comment {

    public Comment() { }

    [Required]
    [Column("CommentID")]
    public int Id { get; set; }

    public string Body { get; set; }
    
    public int? BlogPostId { get; set; }

    [ForeignKey("BlogPostId")] 
    public BlogPost BlogPost { get; set; }

    public string UserId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

  }
}