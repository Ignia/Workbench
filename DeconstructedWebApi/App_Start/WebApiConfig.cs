using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Ignia.Workbench.DeconstructedWebApi {

  /// <summary>
  ///   Provides configuration information for the WebAPI. Referenced by the <see cref="WebApiApplication"/> class as a 
  ///   parameter to <see cref="GlobalConfiguration.Configuration"/>. 
  /// </summary>
  public static class WebApiConfig {

    /// <summary>
    ///   Provides a callback for <see cref="GlobalConfiguration.Configuration"/> to use in order to configure WebAPI, 
    ///   including global defaults and routes. 
    /// </summary>
    /// <param name="config">
    ///   A reference to the global <see cref="HttpConfiguration"/>, which the options will be set on.
    /// </param>
    public static void Register(HttpConfiguration config) {

      //Web API configuration and services
      //Configure Web API to use only bearer token authentication.
      config.SuppressDefaultHostAuthentication();
      config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

      //Allow routes to be overwritten in the controller based on the [Route] attribute
      config.MapHttpAttributeRoutes();

      //Establish default routes for WebAPI based on conventions 
      config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
      );

    }

  } //Class 
} //Namespace
