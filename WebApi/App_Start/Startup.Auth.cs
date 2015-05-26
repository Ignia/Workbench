using Ignia.Workbench.WebApi.Providers;
using Ignia.Workbench.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Constants = Ignia.Workbench.Models.Constants;

namespace Ignia.Workbench.WebApi {

  /*============================================================================================================================
  | CLASS: STARTUP
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Extends the global <see cref="Startup" /> class by providing methods it can use to configure authorization.
  /// </summary>
  public partial class Startup {

    /*==========================================================================================================================
    | OAUTH OPTIONS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the global OAuth options, which are cached for access across the application.
    /// </summary>
    /// <value>The OAuth options.</value>
    public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

    /*==========================================================================================================================
    | PUBLIC CLIENT ID
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the public client identifier.
    /// </summary>
    /// <value>The public client identifier.</value>
    public static string PublicClientId { get; private set; }

    /*==========================================================================================================================
    | CONFIGURE AUTHENTICATION
    \-------------------------------------------------------------------------------------------------------------------------*/

    /// <summary>
    ///   Extends the global <see cref="Startup" /> class to add authentication support to the OWIN pipeline (via the <see
    ///   cref="IAppBuilder" /> instance). For more details on configuring authentication, see <seealso
    ///   cref="http://go.microsoft.com/fwlink/?LinkId=301864" />.
    /// </summary>
    /// <param name="app">The OWIN application pipeline.</param>
    public void ConfigureAuth(IAppBuilder app) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish OWIN Context
      \-----------------------------------------------------------------------------------------------------------------------*/
      //Configure the db context and user manager to use a single instance per request
      app.CreatePerOwinContext(WorkbenchContext.Create);
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      /*------------------------------------------------------------------------------------------------------------------------
      | Configue state storage
      \-----------------------------------------------------------------------------------------------------------------------*/
      //Enable the application to use a cookie to store information for the signed in user and to use a cookie to temporarily
      //store information about a user logging in with a third party login provider
      app.UseCookieAuthentication(new CookieAuthenticationOptions() { });
      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      /*------------------------------------------------------------------------------------------------------------------------
      | Configure OAUTH
      \-----------------------------------------------------------------------------------------------------------------------*/
      PublicClientId = "self";
      OAuthOptions = new OAuthAuthorizationServerOptions {
        TokenEndpointPath = new PathString("/Token"),
        Provider = new ApplicationOAuthProvider(PublicClientId),
        AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
        AllowInsecureHttp = true
      };

      /*------------------------------------------------------------------------------------------------------------------------
      | Enable bearer tokens
      \-----------------------------------------------------------------------------------------------------------------------*/
      app.UseOAuthBearerTokens(OAuthOptions);

      /*------------------------------------------------------------------------------------------------------------------------
      | Configure SSO providers
      \-----------------------------------------------------------------------------------------------------------------------*/
      // Uncomment the following lines to enable logging in with third party login providers
      //app.UseMicrosoftAccountAuthentication(
      //    clientId: "",
      //    clientSecret: "");

      //app.UseTwitterAuthentication(
      //    consumerKey: "",
      //    consumerSecret: "");

      //### NOTE 05192015: Created custom Facebook provider to retrieve email as part of claim
      //### HACK 05202015: Overwrote username with email since a) email claim isn't making it through, and b) Identity expects
      //an email for external usernames.
      var facebookAuthenticationProvider = new FacebookAuthenticationProvider() {
        OnAuthenticated = (context) => {
          var nameClaim = ((ClaimsIdentity)context.Identity).FindFirst(ClaimsIdentity.DefaultNameClaimType);
          ((ClaimsIdentity)context.Identity).RemoveClaim(nameClaim);
          context.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, context.Email));
          return Task.FromResult(0);
        }
      };

      //### NOTE 05192015: Explicitly created options so the scope could be manually set.
      var facebookAuthenticationOptions = new FacebookAuthenticationOptions() {
        AppId = Constants.FacebookAppId,
        AppSecret = Constants.FacebookAppSecret,
        Provider = facebookAuthenticationProvider
      };

      facebookAuthenticationOptions.Scope.Add("email");

      app.UseFacebookAuthentication(facebookAuthenticationOptions);

      //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
      //{
      //    ClientId = "",
      //    ClientSecret = ""
      //});

    }

  } //Class
} //Namespace
