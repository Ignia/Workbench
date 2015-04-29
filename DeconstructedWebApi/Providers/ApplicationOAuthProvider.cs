using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Ignia.Workbench.Models;

namespace Ignia.Workbench.DeconstructedWebApi.Providers {

  /// <summary>
  ///   
  /// </summary>
  public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider {

    private readonly string _publicClientId;

    /// <summary>
    ///   Initializes a new instance of the <see cref="ApplicationOAuthProvider"/> class.
    /// </summary>
    /// <param name="publicClientId">The public client identifier.</param>
    /// <exception cref="System.ArgumentNullException">publicClientId</exception>
    public ApplicationOAuthProvider(string publicClientId) {

      if (publicClientId == null) {
        throw new ArgumentNullException("publicClientId");
      }

      _publicClientId = publicClientId;

    }

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

    /// <summary>
    ///   Validates the client redirect URI.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context) {

      if (context.ClientId == _publicClientId) {
        Uri expectedRootUri = new Uri(context.Request.Uri, "/");

        if (expectedRootUri.AbsoluteUri == context.RedirectUri) {
          context.Validated();
        }
      }

      return Task.FromResult<object>(null);

    }

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