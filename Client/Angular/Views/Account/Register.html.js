(function() {
  'use strict';

  angular
    .module('app')
    .controller('RegisterController', RegisterController);

  RegisterController.$inject = ['$location', 'workbench'];

  function RegisterController($location, workbench) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Register Controller';
	  vm.status  = 'Please complete the following fields to register.';

	  vm.register = register;

    activate();

    function activate() {      

    }

    function register(user) {
      workbench.register(user)
        .then(function(response) {
          vm.status = 'User ' + user.email + 'created.';
          $location.path('/');
        })
        .catch(function(error) {
		      vm.status = "An error occurred: " + error;
	      });
    }
    
  }
})();