using System;
using System.ComponentModel.DataAnnotations;

namespace Ignia.Workbench.DeconstructedWebApi.Models {

  /*============================================================================================================================
  | CLASS: REMOVE LOGIN (BINDING MODEL)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class RemoveLoginBindingModel {

    [Required]
    [Display(Name = "Login provider")]
    public string LoginProvider { get; set; }

    [Required]
    [Display(Name = "Provider key")]
    public string ProviderKey { get; set; }

  } //Class
} //Namespace