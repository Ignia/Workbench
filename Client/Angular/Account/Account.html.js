(function () {
  'use strict';

/*==============================================================================================================================
| Author        Jeremy Caney (Jeremy.Caney@Ignia.com)
| Client        Ignia
| Project       Ignia Workbench
>===============================================================================================================================
| Revisions     Date        Author              Comments
|- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|               29.05.15    Jeremy Caney        Created initial version.
>===============================================================================================================================
| ### TODO JJC052915: Consider breaking up into two controllers, one for authenticated, the other for unauthenticated.
\=============================================================================================================================*/
  angular
    .module('app')
    .controller('AccountController', AccountController);

  AccountController.$inject = ['$route', '$location', 'aspNetIdentity', 'workbench'];

/*==============================================================================================================================
| CONTROLLER: ACCOUNT CONTROLLER
\-----------------------------------------------------------------------------------------------------------------------------*/
/** @ngdoc controller
  * @name app:AccountController
  * @description
  * Provides account management functions for the Account view, including authentication (Login), registration (Register), and 
  * account management (Modify). This includes both support for local accounts (i.e., username and password) as well as third-
  * party accounts (e.g., Facebook, Twitter, etc). 
  */
  function AccountController($route, $location, aspNetIdentity, workbench) {
    /* jshint validthis:true */

  /*============================================================================================================================
  | ESTABLISH VARIABLES
  \---------------------------------------------------------------------------------------------------------------------------*/
    var vm            = this;

    vm.action         = $route.current.params.action.toLowerCase();
    vm.isRegistration = vm.action === 'register';

    vm.providers      = [];
    vm.errors         = [];

    activate();

  /*============================================================================================================================
  | SET STATE BASED ON THE ROUTE'S :ACTION PARAMETER
  >-----------------------------------------------------------------------------------------------------------------------------
  | STATE: REGISTER
  \---------------------------------------------------------------------------------------------------------------------------*/
    if (vm.action === 'register') {
      vm.title = 'Registration';
      vm.status = 'Please complete the following fields to register.';
      vm.externalLoginLabel = 'Or, register using:';
      vm.buttonLabel = 'Register';
      vm.submit = register;
    }

  /*----------------------------------------------------------------------------------------------------------------------------
  | STATE: LOGIN
  \---------------------------------------------------------------------------------------------------------------------------*/
    else if (vm.action === 'login') {
      vm.title = 'Login';
      vm.status = 'Please enter your login credentials';
      vm.externalLoginLabel = 'Or, login using:';
      vm.buttonLabel = 'Login';
      vm.submit = login;
    }

  /*----------------------------------------------------------------------------------------------------------------------------
  | STATE: MODIFY
  >-----------------------------------------------------------------------------------------------------------------------------
  | ### NOTE JJC052915: This state may be further modified based on the results of getAccountInfo().
  \---------------------------------------------------------------------------------------------------------------------------*/
    else if (vm.action === 'modify') {
      vm.title = 'Account';
      vm.status = 'You are currently authenticated using an external login provider. You may optionally add a password to your '
        + 'account below, thus allowing you to login without that provider.';
      vm.externalLoginLabel = 'Or, bind your account to:';
      vm.buttonLabel = 'Save Password';
      vm.hasCredentials = false;
      vm.submit = setPassword;
    }

  /*============================================================================================================================
  | METHOD: ACTIVATE
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#activate
    * @kind  function
    * @description Prepares the Account controller with necessary dependencies. This includes, for instance, a list of external 
    * login providers (e.g., Facebook) that are configured by the backend ASP.NET Identity setup.  
    */
    function activate() {
      processExternalLogin();
      getExternalLogins();
      getAccountInfo();
    }

  /*============================================================================================================================
  | METHOD: REGISTER
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#register
    * @kind  function
    * @description Creates a new user account based on the `Email`, `Password`, and `ConfirmPassword` fields provided by the 
    * form. This method will be bound to the submit button on the `Register` state (based on the `:action` route).
    * 
    * @param {object=} user The user credentials based on the input form data.
    */
    function register(user) {
      aspNetIdentity.register(user)
        .then(function (response) {
          vm.status = 'User ' + user.email + 'created.';
          //$location.path('/');
          login(user);
        })
        .catch(function (error) {
          vm.status = "The following errors were identified with your input:";
          vm.errors = error;
        });
    }

  /*============================================================================================================================
  | METHOD: CHANGE PASSWORD
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#changePassword
    * @kind  function
    * @description Modifies the authenticated user's password based on the `OldPassword`, `NewPassword` and `ConfirmPassword` 
    * fields provided by the form. This method will be bound to the submit button on the `Modify` state (based on the `:action` 
    * route) if the call to {@link aspNetIdentity#manageInfo manageInfo} method returns a `LoginProvider` with the value of 
    * `Local` in its  `Login` collection (thus indicating that the user has established local credentials).
    * 
    * @param {object=} user The user credentials based on the input form data.
    */
    function changePassword(user) {
      aspNetIdentity.changePassword(user)
        .then(function (response) {
          vm.status = 'Password has been changed.';
        })
        .catch(function (error) {
          vm.status = "The following errors were identified with your input:";
          vm.errors = error;
        });
    }

  /*============================================================================================================================
  | METHOD: SET PASSWORD
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#setPassword
    * @kind  function
    * @description Establishes a new password for an existing user based on the `Password` and `ConfirmPassword` fields provided 
    * by the form. This is necessary because users who registered based on an external account (e.g., Facebook) via the {@link
    * aspNetIdentity#registerExternal registerExternal} method will not yet have a password associated with their account. This 
    * method will be bound to the submit button on the `Modify` state (based on the `:action` route) if the call to {@link 
    * aspNetIdentity#manageInfo manageInfo} method doesn't return a `LoginProvider` with the value of  `Local` in its  `Login` 
    * collection (thus indicating that the user has not yet established local credentials).
    * 
    * @param {object=} user The user credentials based on the input form data.
    */
    function setPassword(user) {
      aspNetIdentity.setPassword(user)
        .then(function (response) {
          vm.status = 'Password has been set.';
        })
        .catch(function (error) {
          vm.status = "The following errors were identified with your input:";
          vm.errors = error;
        });
    }

  /*============================================================================================================================
  | METHOD: PROCESS EXTERNAL LOGIN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#processExternalLogin
    * @kind  function
    * @description Looks for the presence of a hash in the URL, as created by one of the ASP.NET Identity registration methods
    * (e.g., {@link aspNetIdentity#register register} or {@link aspNetIdentity#registerExternal registerExternal}). If found, 
    * proceeds to authenticate the user and, if appropriate, either a) create a new account for them (if they aren't already
    * authenticated) or b) link the external account to their existing account (if they are already authenticated).
    */
    function processExternalLogin() {
      aspNetIdentity.processExternalLogin()
        .then(function (response) {
          if (response) {
            vm.status = 'Successfully logged in';
            $location.url('/Posts');
          }
        })
        .catch(function (response) {
          vm.status = "The following errors were identified with your input:";
          vm.errors = response;
        });
    }

  /*============================================================================================================================
  | METHOD: GET ACCOUNT INFO
  >-----------------------------------------------------------------------------------------------------------------------------
  | ### TODO JJC052815: Need to find a cleaner way of handling the #hash exceptions here; this is unattractive. 
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#getAccountInfo
    * @kind  function
    * @description If the user is authenticated, retrieves account information from {@link aspNetIdentity#manageInfo manageInfo}
    * and uses it to determine a) whether or not they have credentials, and b) what external services they are elligible to link
    * their accounts to (i.e., accounts they are already associated with are removed). 
    */
    function getAccountInfo() {
      if (!aspNetIdentity.isAuthenticated()) return;
      aspNetIdentity.manageInfo()
        .then(function (response) {
          vm.manageInfo = response;
          vm.providers = response.ExternalLoginProviders;
          response.Logins.forEach(function (provider) {
            if (provider.LoginProvider === 'Local') {
              if (!$location.hash()) {
                vm.status = 'You may change your password below.';
              }
              vm.hasCredentials = true;
              vm.submit = changePassword;
            }
            vm.providers = vm.providers.filter(function (item) {
              return item.Name !== provider.LoginProvider;
            });
          });
        })
        .catch(function (response) {
          vm.status = "There was an error loading account management info for this profile :(."
        });
    }

  /*============================================================================================================================
  | METHOD: GET EXTERNAL LOGINS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#getAccountInfo
    * @kind  function
    * @description If the user is not authenticated, retrieves a list of external accounts (e.g., Facebook) that the user may
    * authenticate with. For authenticated users, this is handled instead via {@link app:AccountControllers#getAccountInfo
    * getAccountInfo}. 
    */
    function getExternalLogins() {
      if (aspNetIdentity.isAuthenticated()) return;
      aspNetIdentity.getExternalLogins()
        .then(function (response) {
          vm.providers = response;
        })
        .catch(function (response) {
          vm.status = "There was an error loading the providers :(."
        });
    }

  /*============================================================================================================================
  | METHOD: LOGIN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  app:AccountController#login
    * @kind  function
    * @description Authenticates the user based on the `Email` and `Password` fields provided by the form. This method will be 
    * bound to the submit button on the `Login` state (based on the `:action` route). Once authenticated, the user's access 
    * token (as provided by ASP.NET Identity's `/Token` service) will be saved to local storage by the {@link 
    * aspNetIdentity#login login} method.
    * 
    * @param {object=} user The user credentials based on the input form data.
    */
    function login(user) {
      aspNetIdentity.login(user)
        .then(function (response) {
          vm.status = 'Successfully logged in as ' + response.userName;
          $location.path('/Posts');
        })
        .catch(function (error) {
          vm.status = 'An error occurred: ' + error;
        });
    }

  }
})();