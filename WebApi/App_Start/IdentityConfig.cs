﻿using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ignia.Workbench.Models;

namespace Ignia.Workbench.WebApi {

  /*============================================================================================================================
  | CLASS: APPLICATION USER MANAGER
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a custom implementation of the <see cref="UserManager{TUser}"/> class, which may be extended on a per
  ///   application basis to add support for custom properties.
  /// </summary>
  public class ApplicationUserManager : UserManager<User> {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
    /// </summary>
    /// <param name="store">The user store, which will be configured to use the <see cref="User"/> class.</param>
    public ApplicationUserManager(IUserStore<User> store) : base(store) {
    }

    /*==========================================================================================================================
    | CREATE FACTORY METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   A factory method that creates an instance of the <see cref="ApplicationUserManager"/> class based on a set of options
    ///   and an OWIN context.
    /// </summary>
    /// <param name="options">
    ///   Options to be used in configuring this instance of the <see cref="ApplicationUserManager"/> class.
    /// </param>
    /// <param name="context">The OWIN context to be used.</param>
    /// <returns></returns>
    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish variables
      \-----------------------------------------------------------------------------------------------------------------------*/

      //Establishes a new Application User Manager based on the ApplicationUser and ApplicationDbContext classes.
      var manager = new ApplicationUserManager(new UserStore<User>(context.Get<WorkbenchContext>()));

      /*------------------------------------------------------------------------------------------------------------------------
      | Configure validation logic
      \-----------------------------------------------------------------------------------------------------------------------*/

      //Configure validation logic for user names
      manager.UserValidator = new UserValidator<User>(manager) {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      //Configure validation logic for passwords
      manager.PasswordValidator = new PasswordValidator {
        RequiredLength = 6,
        RequireNonLetterOrDigit = true,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
      };

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish data protection
      \-----------------------------------------------------------------------------------------------------------------------*/
      //If a Data Protection Provider was supplied, use it to create the User Token Provider
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null) {
        manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Return user manager
      \-----------------------------------------------------------------------------------------------------------------------*/
      return manager;

    }

  } //Class
} //Namespace
