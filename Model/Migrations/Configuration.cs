namespace Ignia.Workbench.Models.Migrations {
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<Ignia.Workbench.Models.WorkbenchContext> {
    public Configuration() {
      AutomaticMigrationsEnabled = true;
      ContextKey = "Ignia.Workbench.Models.WorkbenchContext";
    }

    protected override void Seed(Ignia.Workbench.Models.WorkbenchContext context) {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.People.AddOrUpdate(
      //      p => p.FullName,
      //      new Person { FullName = "Andrew Peters" },
      //      new Person { FullName = "Brice Lambson" },
      //      new Person { FullName = "Rowan Miller" }
      //    );
      //
    }
  }
}
