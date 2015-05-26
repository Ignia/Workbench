using System;
using System.Collections.Generic;

namespace Ignia.Workbench.WebApi.Models {
  // Models returned by AccountController actions.

  /*============================================================================================================================
  | CLASS: MANAGE INFO (VIEW MODEL)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class ManageInfoViewModel {

    public string LocalLoginProvider { get; set; }

    public string Email { get; set; }

    public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

    public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }

  } //Class
} //Namespace
