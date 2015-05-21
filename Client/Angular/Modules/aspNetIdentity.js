(function() {
  'use strict';

  angular

    .module('aspNetIdentity', [
      // Angular modules 

      // Custom modules 

      // 3rd Party Modules
      'LocalStorageModule'

    ])

    .config(function (localStorageServiceProvider, aspNetIdentityProvider) {
      localStorageServiceProvider.setPrefix('aspNetIdentity');
    })

  	.provider('aspNetIdentity', aspNetIdentityProvider);


  function aspNetIdentityProvider() {
    this.$get = ['$http', '$q', '$location', 'localStorageService', aspNetIdentityFactory];
  };

  function aspNetIdentityFactory($http, $q, $location, localStorageService) {

    var loginProviders;

    return {
      getToken: getToken,
      isAuthenticated: isAuthenticated,
      register: register,
      getLoginProviders: getLoginProviders,
      login: login,
      logout: logout,
      loginExternal: loginExternal
    }

    function getToken() {
      return localStorageService.get('token');
      //return $cookies.get('.AspNet.Cookies');
    }

    function isAuthenticated() {
      return (getToken()) ? true : false;
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
				.success(function (data, status, headers, config) {
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
				  deferred.reject(data.error_description);
				});
      deferred.promise.then(function (response) {
        localStorageService.set('token', response.access_token);
      });
      return deferred.promise;
    }

    function register(user) {
      var deferred = $q.defer();
      $http.post('/API/Account/Register', user)
				.success(function (data, status, headers, config) {
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
				  if (data.ModelState) {
				    deferred.reject(parseErrors(data));
				  }
				  else {
				    deferred.reject(data.Message);
				  }
				});
      return deferred.promise;
    }

    function getLoginProviders() {
      var deferred = $q.defer();
      if (loginProviders) {
        deferred.resolve(loginProviders);
        return deferred.promise;
      }
      $http.get('/API/Account/ExternalLogins?returnUrl=%2FAngular%2FAccount%2FLogin&generateState=true')
				.success(function (data, status, headers, config) {
				  loginProviders = data;
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
				  deferred.reject("Unable to retrieve social logins");
				});
      return deferred.promise;
    }

    function getLoginProviderUrl(name) {
      return getLoginProviders().then(function (data) {
        var output = null;
        data.some(function (provider, index, array) {
          if (provider.Name === name) {
            output = provider.Url;
            return true;
          }
        });
        return output;
      });
    }

    function loginExternal() {
      var deferred = $q.defer();
      var hash = $location.hash();

      if (!hash) {
        deferred.resolve(false);
        return deferred.promise;
      }

      var hashObject = $.parseJSON('{"' + decodeURI(hash).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
      var accessToken = hashObject.access_token;

      $http.get(
			    '/api/Account/UserInfo',
			    {
			      headers: {
			        authorization: 'bearer ' + accessToken
			      }
			    }
		    )
		    .success(function (userInfo, status, headers, config) {
		      if (userInfo.HasRegistered) {
		        deferred.resolve(true);
		        localStorageService.set('token', accessToken);
		      }
		      else {
		        $http.post(
						    '/api/Account/RegisterExternal',
						    {
						      Email: userInfo.Email
						    },
						    {
						      headers: {
						        authorization: 'bearer ' + accessToken
						      }
						    }
					    )
					    .success(function (registerResponse, status, headers, config) {
					      deferred.resolve(registerResponse);
					      getLoginProviderUrl(userInfo.LoginProvider).then(function (url) {
					        window.location.href = decodeURI(url);
					      });
					    })
					    .error(function (errorData, status, headers, config) {
					      if (errorData.ModelState) {
					        deferred.reject(parseErrors(errorData));
					      }
					      else {
					        deferred.reject(errorData.Message);
					      }
					    });
		      }
		    })
		    .error(function (data, status, headers, config) {
		      deferred.reject(data);
		    });
      return deferred.promise;
    }

    function parseErrors(response) {
      var errors = [];
      for (var key in response.ModelState) {
        for (var i = 0; i < response.ModelState[key].length; i++) {
          errors.push(response.ModelState[key][i]);
        }
      }
      return errors;
    }

  }

})();