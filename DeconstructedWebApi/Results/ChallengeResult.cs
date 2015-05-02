using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ignia.Workbench.DeconstructedWebApi.Results {

  /*============================================================================================================================
  | CONSTRUCTOR
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   
  /// </summary>
  public class ChallengeResult : IHttpActionResult {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="ChallengeResult"/> class.
    /// </summary>
    /// <param name="loginProvider">The login provider.</param>
    /// <param name="controller">The controller.</param>
    public ChallengeResult(string loginProvider, ApiController controller) {
      LoginProvider = loginProvider;
      Request = controller.Request;
    }

    /*==========================================================================================================================
    | PROPERTY: LOGIN PROVIDER
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the login provider.
    /// </summary>
    /// <value>The login provider.</value>
    public string LoginProvider { get; set; }

    /*==========================================================================================================================
    | PROPERTY: REQUEST
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the request.
    /// </summary>
    /// <value>The request.</value>
    public HttpRequestMessage Request { get; set; }

    /*==========================================================================================================================
    | METHOD: EXECUTE ASYNCHRONOUSLY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Executes the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
      Request.GetOwinContext().Authentication.Challenge(LoginProvider);

      HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
      response.RequestMessage = Request;
      return Task.FromResult(response);
    }

  } //Class
} //Namespace
