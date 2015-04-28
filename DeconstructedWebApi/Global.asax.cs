using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ignia.Workbench.DeconstructedWebApi {
  public class WebApiApplication : System.Web.HttpApplication {
    protected void Application_Start() {

      //The following ship out-of-the-box with the Web API template, but are not required for a pure Web API project (i.e., w/out documentation).
      //AreaRegistration.RegisterAllAreas();
      //BundleConfig.RegisterBundles(BundleTable.Bundles);
      //RouteConfig.RegisterRoutes(RouteTable.Routes);

      //Configure Web API routes and global settings
      GlobalConfiguration.Configure(WebApiConfig.Register);

      //Add optional global filters to the GlobalFilters repository
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

    }
  }
}
