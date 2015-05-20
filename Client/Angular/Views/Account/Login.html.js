(function() {
  'use strict';

  angular
    .module('app')
    .controller('LoginController', LoginController);

  LoginController.$inject = ['$location', 'workbench'];

  function LoginController($location, workbench) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Login Controller';
	  vm.status  = 'Please enter your login credentials';
    vm.login   = login;
	  vm.providers = [];

    activate();

    function activate() {
	    loginExternal();
	    getLoginProviders();
    }

    function loginExternal() {
	    workbench.loginExternal()
        .then(function(response) {
			    console.log(response);
		    })
        .catch(function(response) {
			    console.log(response);
		    });
    }

    function getLoginProviders() {
	    workbench.getLoginProviders()
		    .then(function(response) {
			    vm.providers = response;
		    })
        .catch(function(response) {
			    vm.status = "There was an error loading the providers :(."
		    });
    }

    function login(user) {
	    workbench.login(user)
		    .then(function(response) {
			    vm.status = 'Successfully logged in as ' + response.userName;
          $location.path('/');
		    })
		    .catch(function(error) {
			    vm.status = 'An error occurred: ' + error;
		    });
    }

  }
})();