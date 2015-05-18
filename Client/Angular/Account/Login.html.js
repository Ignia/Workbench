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

    activate();

    function activate() {      

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