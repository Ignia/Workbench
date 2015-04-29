using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Ignia.Workbench.DeconstructedWebApi.Providers;
using Ignia.Workbench.DeconstructedWebApi.Models;

namespace Ignia.Workbench.DeconstructedWebApi {

  /// <summary>
  ///   Extends the global <see cref="Startup"/> class by providing methods it can use to configure authorization.
  /// </summary>
  public partial class Startup {

    /// <summary>
    ///   Gets the global OAuth options, which are cached for access across the application.
    /// </summary>
    /// <value>The OAuth options.</value>
    public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

    /// <summary>
    ///   Gets the public client identifier.
    /// </summary>
    /// <value>The public client identifier.</value>
    public static string PublicClientId { get; private set; }

    /// <summary>
    ///   Extends the global <see cref="Startup"/> class to add authentication support to the OWIN pipeline (via the 
    ///   <see cref="IAppBuilder"/> instance). For more details on configuring authentication, see 
    ///   <seealso cref="http://go.microsoft.com/fwlink/?LinkId=301864"/>.
    /// </summary>
    /// <param name="app">The OWIN application pipeline.</param>
    public void ConfigureAuth(IAppBuilder app) {

      //Configure the db context and user manager to use a single instance per request
      app.CreatePerOwinContext(ApplicationDbContext.Create);
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      //Enable the application to use a cookie to store information for the signed in user and to use a cookie to temporarily 
      //store information about a user logging in with a third party login provider
      app.UseCookieAuthentication(new CookieAuthenticationOptions());
      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      //Configure the application for OAuth based flow
      PublicClientId = "self";
      OAuthOptions = new OAuthAuthorizationServerOptions {
        TokenEndpointPath = new PathString("/Token"),
        Provider = new ApplicationOAuthProvider(PublicClientId),
        AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
        AllowInsecureHttp = true
      };

      //Enable the application to use bearer tokens to authenticate users
      app.UseOAuthBearerTokens(OAuthOptions);

      // Uncomment the following lines to enable logging in with third party login providers
      //app.UseMicrosoftAccountAuthentication(
      //    clientId: "",
      //    clientSecret: "");

      //app.UseTwitterAuthentication(
      //    consumerKey: "",
      //    consumerSecret: "");

      //app.UseFacebookAuthentication(
      //    appId: "",
      //    appSecret: "");

      //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
      //{
      //    ClientId = "",
      //    ClientSecret = ""
      //});

    }

  } //Class
} //Namespace
