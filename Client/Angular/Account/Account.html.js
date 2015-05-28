(function() {
  'use strict';

  angular
    .module('app')
    .controller('AccountController', AccountController);

  AccountController.$inject = ['$route', '$location', 'aspNetIdentity', 'workbench'];

  function AccountController($route, $location, aspNetIdentity, workbench) {
    /* jshint validthis:true */
    var vm = this;

    vm.isRegistration = $route.current.params.action.match(/register/i);
    
    if (vm.isRegistration) {
      vm.title = 'Registration';
      vm.status = 'Please complete the following fields to register.';
      vm.submit = register;
    }
    else {
      vm.title = 'Login';
      vm.status = 'Please enter your login credentials';
      vm.submit = login;
    }

    vm.providers = [];
	  vm.errors = [];

    activate();

    function activate() {
	    loginExternal();
	    getExternalLogins();
    }


    function register(user) {
      aspNetIdentity.register(user)
        .then(function (response) {
          vm.status = 'User ' + user.email + 'created.';
          $location.path('/');
        })
        .catch(function (error) {
          vm.status = "The following errors were identified with your input:";
          vm.errors = error;
        });
    }

    function loginExternal() {
      aspNetIdentity.loginExternal()
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