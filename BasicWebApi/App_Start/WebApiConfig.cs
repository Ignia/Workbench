using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

using Ignia.Workbench.Models;

namespace BasicWebApi
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services

      ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
      builder.EntitySet<Comment>("Comments");
      builder.EntitySet<Post>("Posts");
      config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );
    }
  }
}
