﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Ignia.Workbench.Models;
using System.Web.OData.Extensions;
using System.Web.OData.Builder;

namespace Ignia.Workbench.WebApi {

  /*============================================================================================================================
  | CLASS: WEB API CONFIGURATION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides configuration information for the WebAPI. Referenced by the <see cref="WebApiApplication"/> class as a 
  ///   parameter to <see cref="GlobalConfiguration.Configuration"/>. 
  /// </summary>
  public static class WebApiConfig {

    /*==========================================================================================================================
    | REGISTER METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a callback for <see cref="GlobalConfiguration.Configuration"/> to use in order to configure WebAPI, 
    ///   including global defaults and routes. 
    /// </summary>
    /// <param name="config">
    ///   A reference to the global <see cref="HttpConfiguration"/>, which the options will be set on.
    /// </param>
    public static void Register(HttpConfiguration config) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Prepare for use of bearer tokens
      \-----------------------------------------------------------------------------------------------------------------------*/
      config.SuppressDefaultHostAuthentication();
      config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

      /*------------------------------------------------------------------------------------------------------------------------
      | Enable RouteAttribute
      \-----------------------------------------------------------------------------------------------------------------------*/
      //Allow routes to be overwritten in the controller based on the [Route] attribute
      config.MapHttpAttributeRoutes();
      config.EnableCaseInsensitive(caseInsensitive: true);

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish WebAPI Routes (uses for /Api/Account/ endpoint)
      \-----------------------------------------------------------------------------------------------------------------------*/
      config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish OData Routes (used for all other endpoints)
      \-----------------------------------------------------------------------------------------------------------------------*/
      ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
      builder.EntitySet<Post>("Posts");
      builder.EntitySet<Comment>("Comments");
      builder.EntitySet<User>("Users");
      config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

    }

  } //Class 
} //Namespace
