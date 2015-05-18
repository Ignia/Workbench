(function() {
  'use strict';

  angular
    .module('app')
    .controller('HomeController', HomeController);

  HomeController.$inject = ['$location', 'workbench'];

  function HomeController($location, workbench) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Home Controller';
	  vm.isAuthenticated = workbench.isAuthenticated;
	  vm.logout = logout;

    activate();

    function activate() {      

    }

	  function logout() {
		  workbench.logout();
		  $location.path('/Account/Login');
	  }

  }
})();