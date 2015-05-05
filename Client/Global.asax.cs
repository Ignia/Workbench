using Ignia.Workbench.DeconstructedWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ignia.Workbench.Client {

  /*============================================================================================================================
  | CLASS: WEB API APPLICATION (HTTP APPLICATION)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class WebApiApplication : System.Web.HttpApplication {

    /*==========================================================================================================================
    | EVENT: APPLICATION START
    \-------------------------------------------------------------------------------------------------------------------------*/
    protected void Application_Start() {

      //Configure Web API routes and global settings
      GlobalConfiguration.Configure(WebApiConfig.Register);

      //Add optional global filters to the GlobalFilters repository
      //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

    }
  }
}
