using System;
using System.ComponentModel.DataAnnotations;

namespace Ignia.Workbench.DeconstructedWebApi.Models {
  // Models used as parameters to AccountController actions.

  /*============================================================================================================================
  | CLASS: ADD EXTERNAL LOGIN (BINDING MODEL)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class AddExternalLoginBindingModel {

    [Required]
    [Display(Name = "External access token")]
    public string ExternalAccessToken { get; set; }

  } //Class
} //Namespace