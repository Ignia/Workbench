using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartup(typeof(Ignia.Workbench.DeconstructedWebApi.Startup))]

namespace Ignia.Workbench.DeconstructedWebApi {

  /*============================================================================================================================
  | CLASS: STARTUP
  \---------------------------------------------------------------------------------------------------------------------------*/
  public partial class Startup {

    /*==========================================================================================================================
    | METHOD: CONFIGURATION
    \-------------------------------------------------------------------------------------------------------------------------*/
    public virtual void Configuration(IAppBuilder app) {
      ConfigureAuth(app);
    }

  } //Class
} //Namespace
