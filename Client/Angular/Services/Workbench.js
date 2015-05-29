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
			getPosts: getPosts,
			getPost: getPost,
		}

    function getPosts() {
			var deferred = $q.defer();
			$http.get(
        '/odata/Posts',
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

    function getPost(postId) {
      var deferred = $q.defer();
      $http.get(
        '/odata/Posts(' + postId + ')/',
				{
				  headers: {
				    authorization: 'bearer ' + aspNetIdentity.getToken()
				  }
				}
      )
				.success(function (data, status, headers, config) {
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
				  deferred.reject("An error occurred loading the data.");
				});
      return deferred.promise;
    }

	}

})();