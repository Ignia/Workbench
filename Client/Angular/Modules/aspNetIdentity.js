(function() {
  'use strict';

/*==============================================================================================================================
| Author        Jeremy Caney (Jeremy.Caney@Ignia.com)
| Client        Ignia
| Project       Workbench
>===============================================================================================================================
| Revisions     Date        Author              Comments
|- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|               21.05.15    Jeremy Caney        Created initial version.
\=============================================================================================================================*/
  angular
    .module('aspNetIdentity', ['LocalStorageModule'])
    .config(function (localStorageServiceProvider, aspNetIdentityProvider) {
      localStorageServiceProvider.setPrefix('aspNetIdentity');
    })
  	.provider('aspNetIdentity', aspNetIdentityProvider);

/*==============================================================================================================================
| PROVIDER: ASP.NET IDENTITY
\-----------------------------------------------------------------------------------------------------------------------------*/
/** @ngdoc provider
  * @name aspNetIdentityProvider
  * @description
  * Provides data access to the OWIN and /Api/Account/ endpoints exposed by the out-of-the-box "Web API" project template.
  */
  function aspNetIdentityProvider() {
    this.$get = ['$http', '$q', '$location', 'localStorageService', aspNetIdentityFactory];
  };

/*==============================================================================================================================
| SERVICE: ASP.NET IDENTITY
\-----------------------------------------------------------------------------------------------------------------------------*/
/** @ngdoc service
  * @name  aspNetIdentityFactory
  * 
  * @requires $http 
  * @requires $q
  * @requires $location
  * @requires localStorageService
  * 
  * @description
  * Provides data access to the OWIN and /Api/Account/ endpoints exposed by the out-of-the-box "Web API" project template.
  */
  function aspNetIdentityFactory($http, $q, $location, localStorageService) {

  /*============================================================================================================================
  | RETURN SERVICE
  \---------------------------------------------------------------------------------------------------------------------------*/
    return {
      getToken                  : getToken                      ,
      isAuthenticated           : isAuthenticated               ,
      register                  : register                      ,
      getLoginProviders         : getLoginProviders             ,
      login                     : login                         ,
      logout                    : logout                        ,
      loginExternal             : loginExternal
    }

  /*============================================================================================================================
  | DECLARE LOCAL VARIABLES
  \---------------------------------------------------------------------------------------------------------------------------*/
    var loginProviders;

  /*============================================================================================================================
  | METHOD: GET TOKEN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getToken
    * @kind  function
    * @description Retrieves the ASP.NET access token from local storage.
    *
    * @return {string} The ASP.NET access token.
    */
    function getToken() {
      return localStorageService.get('token');
      //return $cookies.get('.AspNet.Cookies');
    }

  /*============================================================================================================================
  | METHOD: IS AUTHENTICATED?
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#isAuthenticated
    * @kind  function
    * @description Determines whether the current user is authenticated or not by evaluating the presence of an access token.
    *
    * @return {bool} The user's authentication status.
    */
    function isAuthenticated() {
      return (getToken()) ? true : false;
    }

  /*============================================================================================================================
  | METHOD: LOGOUT
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#logout
    * @kind  function
    * @description Logs the user out by removing their access token, and clearing any other state variables.
    */
    function logout() {
      localStorageService.remove('token');
    }

  /*============================================================================================================================
  | METHOD: LOGIN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#login
    * @kind  function
    * @description Logs a user into the system with their username (email) and password credentials via OWIN's `/Token` endpoint. 
    *
    * @param {object=} user An object containing an `Email` property and a `Password` property.  
    *
    * @return {promise} The login callback promise.
    */
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

  /*============================================================================================================================
  | METHOD: REGISTER
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#register
    * @kind  function
    * @description Registers a user based on a username (email) and password via ASP.NET's `/API/Account/Register` endpoint.
    *
    * @param {object=} user An object containing an `Email` property, `Password` property, and `ConfirmPassword` property.  
    *
    * @return {promise} The registration callback promise.
    */
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

  /*============================================================================================================================
  | METHOD: GET LOGIN PROVIDERS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getLoginProviders
    * @kind  function
    * @description Retrieves a list of available third-party login providers, such as Facebook or Twitter, which the application 
    * is configured to accept. Each contains a name and a URL, which should be presented to the user as a list of login links. 
    * The results are cached locally.
    *
    * @return {promise} The login provider's callback promise.
    */
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

  /*============================================================================================================================
  | METHOD: GET LOGIN PROVIDER URL
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getLoginProviderUrl
    * @kind  function
    * @description Given a login provider name (e.g., `Facebook`) will lookup the provider via the 
    * {@link aspNetIdentity#getLoginProviders getLoginProviders} method.
    *
    * @return {string} The URL of the login provider, if found; otherwise `null`.
    */
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

  /*============================================================================================================================
  | METHOD: LOGIN EXTERNAL
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#loginExternal
    * @kind  function
    * @description Checks for the presence of an access token (via the `#access_token` hash parameter) generated on for users 
    * authenticated via a third-party service (such as Facebook). If found, the access token is extracted and used to determine 
    * if the user has registered or not. If the user has not registered, they are registered via Web API's 
    * `/API/Account/RegisterExternal` service. Once that's done, the user is sent back through the login service (retrieved 
    * using the {@link aspNetIdentity#getLoginServiceUrl getLoginServiceUrl()} method), which then generates a final access 
    * token. At that point, this method can be called again to log the user in by storing their access token via local storage. 
    *
    * @return {promise} The external login callback promise.
    */
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

  /*============================================================================================================================
  | METHOD: PARSE ERRORS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#parseErrors
    * @kind  function
    * @description Parses a standard error response from Web API into a string array so that it can be easily consumed by 
    * Angular via an {@link ng.ngRepeat ngRepeat} directive. It is specifically intended to operate with `ModelState` 
    * errors. As part of this process, the error data is flattened and the context (e.g., associated property name) is 
    * removed.
    *
    * @return {array} A string array of error messages.
    */
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