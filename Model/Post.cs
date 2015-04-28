using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {

  public class Post {
    public Post() { }

    [Column("PostID")]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Body { get; set; }

    public string Keywords { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

    public DateTime DateCreated { get; set; }

  }
}
