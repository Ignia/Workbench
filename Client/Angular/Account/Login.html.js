(function() {
  'use strict';

  angular
    .module('app')
    .controller('LoginController', LoginController);

  LoginController.$inject = ['$location', 'aspNetIdentity', 'workbench'];

  function LoginController($location, aspNetIdentity, workbench) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Login';
	  vm.status  = 'Please enter your login credentials';
    vm.login   = login;
	  vm.providers = [];
	  vm.errors = [];

    activate();

    function activate() {
	    loginExternal();
	    getLoginProviders();
    }

    function loginExternal() {
      aspNetIdentity.loginExternal()
        .then(function(response) {
          if (response) {
			      vm.status = 'Successfully logged in';
            $location.path('/Posts');
          }
		    })
        .catch(function(response) {
			    vm.status = "The following errors were identified with your input:";
			    vm.errors = response;
		    });
    }

    function getLoginProviders() {
      aspNetIdentity.getLoginProviders()
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