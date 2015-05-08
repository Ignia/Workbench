(function() {
	'use strict';

	angular
		.module('app')
		.provider('workbench', workbenchProvider)
		.config(function(workbenchProvider) {});

	function workbenchProvider() {
		this.$get = ['$http', '$q', workbenchFactory];
  };

	function workbenchFactory($http, $q) {

		return {
			someValue: function() { return 'a different value' },
			getData: getData
 	  }

    function getData() {
			var deferred = $q.defer();
			$http.get('/odata/Posts?$expand=Comments')
				.success(function(data, status, headers, config) {
					deferred.resolve(data.value);
				})
				.error(function(data, status, headers, config) {
					deferred.reject("An error occurred loading the data.");
				});
			return deferred.promise;
		}

	}

})();