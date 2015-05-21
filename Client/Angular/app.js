(function() {
  'use strict';

  angular

    .module('app', [
      // Angular modules 
      'ngRoute',
      'ngCookies',

      // Custom modules 
      'aspNetIdentity',

      // 3rd Party Modules
      'LocalStorageModule'

    ])

    .config(function ($locationProvider) {
      $locationProvider.html5Mode(true);
    });

})();