using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Ignia.Workbench.DeconstructedWebApi.Startup))]

namespace Ignia.Workbench.DeconstructedWebApi {

  /*============================================================================================================================
  | CLASS: STARTUP
  \---------------------------------------------------------------------------------------------------------------------------*/
  public partial class Startup {

    /*==========================================================================================================================
    | METHOD: CONFIGURATION
    \-------------------------------------------------------------------------------------------------------------------------*/
    public void Configuration(IAppBuilder app) {
      ConfigureAuth(app);
    }

  } //Class
} //Namespace 
