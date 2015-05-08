(function() {
  'use strict';

  angular
    .module('app')
    .controller('HomeController', HomeController);

  HomeController.$inject = ['$location', 'workbench', '$q'];

  function HomeController($location, workbench, $q) {
    /* jshint validthis:true */
    var vm = this;
    
    vm.title   = 'Home Controller';
    vm.someValue = workbench.someValue();

    workbench.getData().then(function(data) {
      vm.posts = data;
    });

    activate();

    function activate() {      

    }
  }
})();