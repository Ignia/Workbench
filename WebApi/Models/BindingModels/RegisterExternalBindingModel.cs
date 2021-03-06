﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Ignia.Workbench.WebApi.Models {

  /*============================================================================================================================
  | CLASS: EXTERNAL REGISTRATION (BINDING MODEL)
  \---------------------------------------------------------------------------------------------------------------------------*/
  public class RegisterExternalBindingModel {

    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }

  } //Class
} //Namespace