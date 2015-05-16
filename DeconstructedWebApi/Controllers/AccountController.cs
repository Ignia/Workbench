using Ignia.Workbench.DeconstructedWebApi.Models;
using Ignia.Workbench.DeconstructedWebApi.Providers;
using Ignia.Workbench.DeconstructedWebApi.Results;
using Ignia.Workbench.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Ignia.Workbench.DeconstructedWebApi.Controllers {

  /// <summary>
  ///   The <see cref="AccountController" /> provides support for all account functions, including registration, authentication,
  ///   and retrieving user information. It is responsible for all requests to the
  ///   <code>
  ///     /api/Account
  ///   </code>endpoint.
  /// </summary>
  [Authorize]
  [RoutePrefix("api/Account")]
  public class AccountController : ApiController {

    private const string LocalLoginProvider = "Local";
    private ApplicationUserManager _userManager;

    /// <summary>
    ///   Initializes a new instance of the <see cref="AccountController" /> class.
    /// </summary>
    public AccountController() {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="AccountController" /> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="accessTokenFormat">The access token format.</param>
    public AccountController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat) {
      UserManager = userManager;
      AccessTokenFormat = accessTokenFormat;
    }

    /// <summary>
    ///   Gets or sets a reference to the user manager configured for this controller; if no user manager is configured, then it
    ///   retrieves one from the current request's OWIN context.
    /// </summary>
    /// <value>The user manager.</value>
    public ApplicationUserManager UserManager {
      get {
        return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
      private set {
        _userManager = value;
      }
    }

    /// <summary>
    ///   Gets the access token format.
    /// </summary>
    /// <value>The access token format.</value>
    public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

    /// <summary>
    ///   Gets information about the currently authenticated user via
    ///   <code>
    ///     GET
    ///   </code>requests to the
    ///   <code>
    ///     /api/Account/UserInfo
    ///   </code>endpoint.
    /// </summary>
    /// <returns>Information about the currently authenticated user</returns>
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [Route("UserInfo")]
    public UserInfoViewModel GetUserInfo() {
      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

      return new UserInfoViewModel {
        Email = User.Identity.GetUserName(),
        HasRegistered = externalLogin == null,
        LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
      };
    }

    /// <summary>
    ///   Logs out the currently authenticated user via
    ///   <code>
    ///     POST
    ///   </code>requests to the
    ///   <code>
    ///     /api/Account/Logout
    ///   </code>endpoint.
    /// </summary>
    /// <returns></returns>
    [Route("Logout")]
    public IHttpActionResult Logout() {
      Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
      return Ok();
    }

    // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
    /// <summary>
    ///   Gets the manage information.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <param name="generateState">if set to <c>true</c> [generate state].</param>
    /// <returns></returns>
    [Route("ManageInfo")]
    public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false) {
      IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

      if (user == null) {
        return null;
      }

      List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

      foreach (IdentityUserLogin linkedAccount in user.Logins) {
        logins.Add(new UserLoginInfoViewModel {
          LoginProvider = linkedAccount.LoginProvider,
          ProviderKey = linkedAccount.ProviderKey
        });
      }

      if (user.PasswordHash != null) {
        logins.Add(new UserLoginInfoViewModel {
          LoginProvider = LocalLoginProvider,
          ProviderKey = user.UserName,
        });
      }

      return new ManageInfoViewModel {
        LocalLoginProvider = LocalLoginProvider,
        Email = user.UserName,
        Logins = logins,
        ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
      };
    }

    /// <summary>
    ///   Changes the password for the currently authenticated user via a <c>POST</c> request to the
    ///   <c>/api/Account/ChangePassword</c> endpoint.
    /// </summary>
    /// <param name="model">The binding model including the parameters for changing a password.</param>
    /// <returns></returns>
    [Route("ChangePassword")]
    public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model) {

      //Validate the current request
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      //Make an asyncronous request to change the password via the UserManager
      IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

      //Handle errors
      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      //Handle a successful password change
      return Ok();
    }

    /// <summary>
    ///   Sets the password of the currently authenticated user via a <c>POST</c> request to the <c>/api/Account/SetPassword</c> endpoint.
    /// </summary>
    /// <param name="model">The binding model for setting the password.</param>
    /// <returns></returns>
    [Route("SetPassword")]
    public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model) {

      //Validate the current request
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      //Make an asynchronous call to change the password
      IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

      //Handle errors
      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      //Handle a successful password change
      return Ok();
    }

    // POST api/Account/AddExternalLogin
    /// <summary>
    ///   Associates an external login (e.g., Facebook) with an existing account.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    [Route("AddExternalLogin")]
    public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model) {

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

      AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

      if (
        ticket == null ||
        ticket.Identity == null ||
        (
          ticket.Properties != null
          && ticket.Properties.ExpiresUtc.HasValue
          && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow
        )
      ) {
        return BadRequest("External login failure.");
      }

      ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

      if (externalData == null) {
        return BadRequest("The external login is already associated with an account.");
      }

      IdentityResult result = await UserManager.AddLoginAsync(
        User.Identity.GetUserId(),
        new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey)
      );

      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // POST api/Account/RemoveLogin
    /// <summary>
    ///   Removes the login.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    [Route("RemoveLogin")]
    public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      IdentityResult result;

      if (model.LoginProvider == LocalLoginProvider) {
        result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
      }
      else {
        result = await UserManager.RemoveLoginAsync(
          User.Identity.GetUserId(),
          new UserLoginInfo(model.LoginProvider, model.ProviderKey)
        );
      }

      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // GET api/Account/ExternalLogin
    /// <summary>
    ///   Gets the external login.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="error">The error.</param>
    /// <returns></returns>
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
    [AllowAnonymous]
    [Route("ExternalLogin", Name = "ExternalLogin")]
    public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null) {
      if (error != null) {
        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
      }

      if (!User.Identity.IsAuthenticated) {
        return new ChallengeResult(provider, this);
      }

      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

      if (externalLogin == null) {
        return InternalServerError();
      }

      if (externalLogin.LoginProvider != provider) {
        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        return new ChallengeResult(provider, this);
      }

      User user = await UserManager.FindAsync(
        new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey)
      );

      bool hasRegistered = user != null;

      if (hasRegistered) {

        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager, OAuthDefaults.AuthenticationType);
        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager, CookieAuthenticationDefaults.AuthenticationType);

        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
      }
      else {
        IEnumerable<Claim> claims = externalLogin.GetClaims();
        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        Authentication.SignIn(identity);
      }

      return Ok();
    }

    // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
    /// <summary>
    ///   Gets the external logins.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <param name="generateState">if set to <c>true</c> [generate state].</param>
    /// <returns></returns>
    [AllowAnonymous]
    [Route("ExternalLogins")]
    public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false) {

      IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
      List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();
      string state;

      if (generateState) {
        const int strengthInBits = 256;
        state = RandomOAuthStateGenerator.Generate(strengthInBits);
      }
      else {
        state = null;
      }

      foreach (AuthenticationDescription description in descriptions) {
        ExternalLoginViewModel login = new ExternalLoginViewModel {
          Name = description.Caption,
          Url = Url.Route(
            "ExternalLogin",
            new {
              provider = description.AuthenticationType,
              response_type = "token",
              client_id = Startup.PublicClientId,
              redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
              state = state
            }
          ),
          State = state
        };
        logins.Add(login);
      }

      return logins;
    }

    // POST api/Account/Register
    /// <summary>
    ///   Registers the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    [AllowAnonymous]
    [Route("Register")]
    public async Task<IHttpActionResult> Register(RegisterBindingModel model) {

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      var user = new User() { UserName = model.Email, Email = model.Email };

      IdentityResult result = await UserManager.CreateAsync(user, model.Password);

      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // POST api/Account/RegisterExternal
    /// <summary>
    ///   Registers the external.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [Route("RegisterExternal")]
    public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model) {

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      var info = await Authentication.GetExternalLoginInfoAsync();
      if (info == null) {
        return InternalServerError();
      }

      var user = new User() { UserName = model.Email, Email = model.Email };

      IdentityResult result = await UserManager.CreateAsync(user);
      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      result = await UserManager.AddLoginAsync(user.Id, info.Login);
      if (!result.Succeeded) {
        return GetErrorResult(result);
      }

      return Ok();

    }

    /// <summary>
    ///   Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing) {

      if (disposing && _userManager != null) {
        _userManager.Dispose();
        _userManager = null;
      }

      base.Dispose(disposing);
    }

    #region Helpers

    /// <summary>
    ///   Gets the authentication.
    /// </summary>
    /// <value>The authentication.</value>
    private IAuthenticationManager Authentication {
      get { return Request.GetOwinContext().Authentication; }
    }

    /// <summary>
    ///   Gets the error result.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    private IHttpActionResult GetErrorResult(IdentityResult result) {
      if (result == null) {
        return InternalServerError();
      }

      if (!result.Succeeded) {
        if (result.Errors != null) {
          foreach (string error in result.Errors) {
            ModelState.AddModelError("", error);
          }
        }

        if (ModelState.IsValid) {
          // No ModelState errors are available to send, so just return an empty BadRequest.
          return BadRequest();
        }

        return BadRequest(ModelState);
      }

      return null;
    }

    /// <summary>
    ///   </summary>
    private class ExternalLoginData {
      public string LoginProvider { get; set; }

      public string ProviderKey { get; set; }

      public string UserName { get; set; }

      public IList<Claim> GetClaims() {
        IList<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        if (UserName != null) {
          claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        }

        return claims;
      }

      /// <summary>
      ///   Froms the identity.
      /// </summary>
      /// <param name="identity">The identity.</param>
      /// <returns></returns>
      public static ExternalLoginData FromIdentity(ClaimsIdentity identity) {
        if (identity == null) {
          return null;
        }

        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
            || String.IsNullOrEmpty(providerKeyClaim.Value)) {
          return null;
        }

        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer) {
          return null;
        }

        return new ExternalLoginData {
          LoginProvider = providerKeyClaim.Issuer,
          ProviderKey = providerKeyClaim.Value,
          UserName = identity.FindFirstValue(ClaimTypes.Name)
        };
      }
    }

    /// <summary>
    ///   </summary>
    private static class RandomOAuthStateGenerator {

      private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

      public static string Generate(int strengthInBits) {
        const int bitsPerByte = 8;

        if (strengthInBits % bitsPerByte != 0) {
          throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
        }

        int strengthInBytes = strengthInBits / bitsPerByte;

        byte[] data = new byte[strengthInBytes];
        _random.GetBytes(data);
        return HttpServerUtility.UrlTokenEncode(data);
      }
    }

    #endregion Helpers
  }
}
