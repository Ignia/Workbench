using System;
using System.ComponentModel.DataAnnotations;

namespace Ignia.Workbench.DeconstructedWebApi.Models {

  public class RemoveLoginBindingModel {
    [Required]
    [Display(Name = "Login provider")]
    public string LoginProvider { get; set; }

    [Required]
    [Display(Name = "Provider key")]
    public string ProviderKey { get; set; }
  }

} //Namespace