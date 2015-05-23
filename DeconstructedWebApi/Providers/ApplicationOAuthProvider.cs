using Ignia.Workbench.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ignia.Workbench.DeconstructedWebApi.Providers {

  /*============================================================================================================================
  | CLASS: APPLICATION OAUTH PROVIDER
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   </summary>
  public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider {

    /*==========================================================================================================================
    | DECLARE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    private readonly string _publicClientId;

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="ApplicationOAuthProvider" /> class.
    /// </summary>
    /// <param name="publicClientId">The public client identifier.</param>
    /// <exception cref="System.ArgumentNullException">publicClientId</exception>
    public ApplicationOAuthProvider(string publicClientId) {

      if (publicClientId == null) {
        throw new ArgumentNullException("publicClientId");
      }

      _publicClientId = publicClientId;

    }

    /*==========================================================================================================================
    | GRANT RESOURCE OWNER CREDENTIALS METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Grants the resource owner credentials.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) {

      var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

      User user = await userManager.FindAsync(context.UserName, context.Password);

      if (user == null) {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        return;
      }

      ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
      ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(
        userManager,
        CookieAuthenticationDefaults.AuthenticationType
        );

      AuthenticationProperties properties = CreateProperties(user.UserName);
      AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
      context.Validated(ticket);
      context.Request.Context.Authentication.SignIn(cookiesIdentity);

    }

    /*==========================================================================================================================
    | AUTHROIZATION ENDPOINT RESPONSE
    >---------------------------------------------------------------------------------------------------------------------------
    | ### NOTE JJC05192015: Approach borrowed from Rahul Nath's blog post:
    | http://www.rahulpnath.com/blog/asp-dot-net-web-api-and-external-login-authenticating-with-social-networks/
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Manages the response from the authorization endpoint (e.g., /Api/Account/ExternalLogin/).
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context) {
      // Add the claims to the return url
      foreach (var claim in context.Identity.Claims) {
        if (claim.Type == ClaimsIdentity.DefaultNameClaimType) {
          context.AdditionalResponseParameters.Add("username", claim.Value);
        }
      }
      return base.AuthorizationEndpointResponse(context);
    }

    /*==========================================================================================================================
    | TOKEN ENDPOINT METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tokens the endpoint.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override Task TokenEndpoint(OAuthTokenEndpointContext context) {

      foreach (KeyValuePair<string, string> property in context.Properties.Dictionary) {
        context.AdditionalResponseParameters.Add(property.Key, property.Value);
      }

      return Task.FromResult<object>(null);

    }

    /*==========================================================================================================================
    | VALIDATE CLIENT AUTHENTICATION METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Validates the client authentication.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {

      // Resource owner password credentials does not provide a client ID.
      if (context.ClientId == null) {
        context.Validated();
      }

      return Task.FromResult<object>(null);

    }

    /*==========================================================================================================================
    | VALIDATE CLIENT REDIRECT URI METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Validates the client redirect URI.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context) {

      if (context.ClientId == _publicClientId) {
        Uri expectedRootUri = new Uri(context.Request.Uri, "/");

        //### NOTE JJC05182015: Updated validation logic to use StartsWith() instead of Equals() to allow all child URLs.
        if (context.RedirectUri.StartsWith(expectedRootUri.AbsoluteUri)) {
          context.Validated();
        }
      }

      return Task.FromResult<object>(null);

    }

    /*==========================================================================================================================
    | CREATE PROPERTIES METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates the properties.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns></returns>
    public static AuthenticationProperties CreateProperties(string userName) {

      IDictionary<string, string> data = new Dictionary<string, string> {
        { "userName", userName }
      };

      return new AuthenticationProperties(data);

    }

  } //Class
} //Namespace
