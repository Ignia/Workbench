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
    vm.someValue = workbench.someValue();
    vm.posts   = workbench.getData();

    activate();

    function activate() {      

    }
  }
})();