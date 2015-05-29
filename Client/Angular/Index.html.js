(function() {
  'use strict';

  angular
    .module('app')
    .controller('HomeController', HomeController);

  HomeController.$inject = ['$location', 'aspNetIdentity', 'workbench'];

  function HomeController($location, aspNetIdentity, workbench) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Home Controller';
    vm.isAuthenticated = aspNetIdentity.isAuthenticated;
    vm.logout = logout;

    activate();

    function activate() {      

    }

    function logout() {
      aspNetIdentity.logout();
      $location.url('/Account/Login');
    }

  }
})();