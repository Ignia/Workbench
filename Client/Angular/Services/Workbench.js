(function() {
	'use strict';

	angular
		.module('app')
		.provider('workbench', workbenchProvider)
		.config(function(workbenchProvider) {});

	function workbenchProvider() {
		this.$get = ['$http', '$q', '$cookies', '$location', 'aspNetIdentity', workbenchFactory];
  };

	function workbenchFactory($http, $q, $cookies, $location, aspNetIdentity) {

		var loginProviders;

		return {
			someValue: function() { return 'a different value' },
			getPosts: getPosts,
 	  }

    function getPosts() {
			var deferred = $q.defer();
			$http.get(
        '/odata/Posts?$expand=Comments',
				{
				  headers: {
					  authorization: 'bearer ' + aspNetIdentity.getToken()
				  }		
				}
      )
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