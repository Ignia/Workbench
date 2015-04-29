using System;
using System.Collections.Generic;

namespace Ignia.Workbench.DeconstructedWebApi.Models {
  // Models returned by AccountController actions.

  public class ManageInfoViewModel {
    public string LocalLoginProvider { get; set; }

    public string Email { get; set; }

    public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

    public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
  }

}
