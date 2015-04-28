using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {
  public class Comment {

    public Comment() { }

    [Column("CommentID")]
    public int Id { get; set; }

    [Required]
    public string Body { get; set; }

    [Required]
    public int PostId { get; set; }

    public virtual Post Post { get; set; }

    public DateTime DateCreated { get; set; }

  }
}
