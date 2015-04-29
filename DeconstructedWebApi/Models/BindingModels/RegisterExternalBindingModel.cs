using System;
using System.ComponentModel.DataAnnotations;

namespace Ignia.Workbench.DeconstructedWebApi.Models {

  public class RegisterExternalBindingModel {
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }

} //Namespace