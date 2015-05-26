using System;
using System.Collections.Generic;

namespace Ignia.Workbench.WebApi.Models {
  // Models returned by AccountController actions.

  /*============================================================================================================================
  | CLASS: EXTERNAL LOGIN (VIEW MODEL)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class ExternalLoginViewModel {

    public string Name { get; set; }

    public string Url { get; set; }

    public string State { get; set; }

  } //Class
} //Namespace
