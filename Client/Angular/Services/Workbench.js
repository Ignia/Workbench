(function() {
	'use strict';

	angular
		.module('app')
		.provider('workbench', workbenchProvider)
		.config(function(workbenchProvider) {});

	function workbenchProvider() {
		this.$get = ['$http', '$q', '$location', 'localStorageService', workbenchFactory];
  };

	function workbenchFactory($http, $q, $location, localStorageService) {

		return {
			someValue: function() { return 'a different value' },
			getToken: getToken,
			isAuthenticated: isAuthenticated,
			getPosts: getPosts,
      register: register,
      login: login,
      logout: logout 
 	  }

    function getToken() {
	    return localStorageService.get('token');
    }

    function isAuthenticated() {
	    return (getToken())? true : false;
    }

    function getPosts() {
			var deferred = $q.defer();
			$http.get(
        '/odata/Posts?$expand=Comments',
				{
				  headers: {
					  authorization: 'bearer ' + getToken()
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

		function logout() {
			localStorageService.remove('token');
		}

		function login(user) {
			var deferred = $q.defer();
			$http.post(
        '/Token',
			  $.param({
				  'grant_type': 'password',
          'username': user.Email,
          'password': user.Password
			  }),
			  {
          'headers': {
	           'Content-Type': 'application/x-www-form-urlencoded'
          }
				}
      )
				.success(function(data, status, headers, config) {
					deferred.resolve(data);
				})
				.error(function(data, status, headers, config) {
  				deferred.reject(data.error_description);
				});
	    deferred.promise.then(function(response) {
		    localStorageService.set('token', response.access_token);
	    });
			return deferred.promise;
		}

    function register(user) {
			var deferred = $q.defer();
			$http.post('/API/Account/Register', user)
				.success(function(data, status, headers, config) {
					deferred.resolve(data);
				})
				.error(function(data, status, headers, config) {
					if (data.ModelState) {
						deferred.reject(data.ModelState[''][0]);
					}
					else {
						deferred.reject(data.Message);
					}
				});
			return deferred.promise;
		}


	}

})();