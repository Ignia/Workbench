using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models
{
  public class WorkbenchContext : DbContext
  {

    public WorkbenchContext() : base("Workbench") { }

    public System.Data.Entity.DbSet<Ignia.Workbench.Models.Post> Posts { get; set; }

    public System.Data.Entity.DbSet<Ignia.Workbench.Models.Comment> Comments { get; set; }
  }
}
