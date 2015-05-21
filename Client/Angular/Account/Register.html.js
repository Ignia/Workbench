(function() {
  'use strict';

  angular
    .module('app')
    .controller('RegisterController', RegisterController);

  RegisterController.$inject = ['$location', 'aspNetIdentity', 'workbench'];

  function RegisterController($location, aspNetIdentity, workbench) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Registration';
	  vm.status  = 'Please complete the following fields to register.';
    vm.errors  = [];

	  vm.register = register;

    activate();

    function activate() {      

    }

    function register(user) {
      aspNetIdentity.register(user)
        .then(function(response) {
          vm.status = 'User ' + user.email + 'created.';
          $location.path('/');
        })
        .catch(function(error) {
			    vm.status = "The following errors were identified with your input:";
			    vm.errors = response;
	      });
    }
    
  }
})();