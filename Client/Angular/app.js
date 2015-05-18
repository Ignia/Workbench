(function() {
  'use strict';

  angular

    .module('app', [
      // Angular modules 
      'ngRoute',

      // Custom modules 

      // 3rd Party Modules
      'LocalStorageModule'

    ])

    .config(function (localStorageServiceProvider) {
      localStorageServiceProvider.setPrefix('workbench');
    });

})();