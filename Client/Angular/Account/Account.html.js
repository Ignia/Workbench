(function() {
  'use strict';

  angular
    .module('app')
    .controller('AccountController', AccountController);

  AccountController.$inject = ['$route', '$location', 'aspNetIdentity', 'workbench'];

  function AccountController($route, $location, aspNetIdentity, workbench) {
    /* jshint validthis:true */
    var vm = this;

    vm.action = $route.current.params.action.toLowerCase();
    vm.isRegistration = vm.action === 'register';

    if (vm.action === 'register') {
      vm.title = 'Registration';
      vm.status = 'Please complete the following fields to register.';
      vm.externalLoginLabel = 'Or, register using:';
      vm.buttonLabel = 'Register';
      vm.submit = register;
    }
    else if (vm.action === 'login') {
      vm.title = 'Login';
      vm.status = 'Please enter your login credentials';
      vm.externalLoginLabel = 'Or, login using:';
      vm.buttonLabel = 'Login';
      vm.submit = login;
    }
    else if (vm.action === 'modify') {
      vm.title = 'Account';
      vm.status = 'You are currently authenticated using an external login provider. You may optionally add a password to your account below, thus allowing you to login without that provider.';
      vm.externalLoginLabel = 'Or, bind your account to:';
      vm.buttonLabel = 'Save Password';
      vm.hasCredentials = false;
      vm.submit = setPassword;
    }

    vm.providers = [];
	  vm.errors = [];

    activate();

    function activate() {
	    processExternalLogin();
	    getExternalLogins();
      getAccountInfo();
    }

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

    function processExternalLogin() {
      aspNetIdentity.processExternalLogin()
        .then(function(response) {
          if (response) {
			      vm.status = 'Successfully logged in';
            $location.url('/Posts');
          }
		    })
        .catch(function(response) {
			    vm.status = "The following errors were identified with your input:";
			    vm.errors = response;
		    });
    }

    ///### TODO JJC052815: Need to find a cleaner way of handling the #hash exceptions here; this is unattractive. 
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
            if (!$location.hash()) {
              vm.providers = vm.providers.filter(function (item) {
                return item.Name !== provider.LoginProvider;
              });
            }
          });
        })
        .catch(function (response) {
          vm.status = "There was an error loading account management info for this profile :(."
        });
    }

    function getExternalLogins() {
      aspNetIdentity.getExternalLogins()
		    .then(function(response) {
			    vm.providers = response;
		    })
        .catch(function(response) {
			    vm.status = "There was an error loading the providers :(."
		    });
    }

    function login(user) {
      aspNetIdentity.login(user)
		    .then(function(response) {
			    vm.status = 'Successfully logged in as ' + response.userName;
          $location.path('/Posts');
		    })
		    .catch(function(error) {
			    vm.status = 'An error occurred: ' + error;
		    });
    }

  }
})();