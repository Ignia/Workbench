using System;
using System.ComponentModel.DataAnnotations;

namespace Ignia.Workbench.DeconstructedWebApi.Models {

  /*============================================================================================================================
  | CLASS: EXTERNAL REGISTRATION (BINDING MODEL)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class RegisterExternalBindingModel {

    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }

  } //Class
} //Namespace