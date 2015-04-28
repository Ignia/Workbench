using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Ignia.Workbench.DeconstructedWebApi.Startup))]

namespace Ignia.Workbench.DeconstructedWebApi {
  public partial class Startup {
    public void Configuration(IAppBuilder app) {
      ConfigureAuth(app);
    }
  }
}
