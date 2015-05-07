(function() {
	'use strict';

	angular
		.module('app')
		.factory('workbench', workbench);

	workbench.$inject = ['$http', '$q'];

	function workbench($http, $q) {
		var service = {
			getData: getData,
      someValue: function() { return 'a different value' }
		};

		return service;

		function getData() {
			var deferred = $q.defer();
      $http.get('/odata/Posts/?$expand=Comments')
        .success(function(data, status, headers, config) {
		      deferred.resolve(data);
	      })
        .error(function(data, status, headers, config) {
		      deferred.reject("An error occurred loading the data.");
	      });
		}

	}
})();