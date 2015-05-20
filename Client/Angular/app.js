(function() {
  'use strict';

  angular

    .module('app', [
      // Angular modules 
      'ngRoute',
      'ngCookies',

      // Custom modules 

      // 3rd Party Modules
      'LocalStorageModule'

    ])

    .config(function (localStorageServiceProvider, $locationProvider) {
      localStorageServiceProvider.setPrefix('workbench');
      $locationProvider.html5Mode(true);
    });

})();