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
  | DECLARE LOCAL VARIABLES
  \---------------------------------------------------------------------------------------------------------------------------*/
    var loginProviders;

  /*============================================================================================================================
  | RETURN SERVICE
  \---------------------------------------------------------------------------------------------------------------------------*/
    return {
      getToken                  : getToken                      ,
      isAuthenticated           : isAuthenticated               ,
      getUserInfo               : getUserInfo                   ,
      login                     : login                         ,
      loginExternal             : loginExternal                 ,
      register                  : register                      ,
      registerExternal          : registerExternal              ,
      getLoginProviders         : getLoginProviders             ,
      getLoginProviderUrl       : getLoginProviderUrl           ,
      logout                    : logout                        
    }

  /*============================================================================================================================
  | METHOD: GET TOKEN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getToken
    * @kind  function
    * @description Retrieves the ASP.NET access token from either the hash, for external logins, or from the local storage, for
    * persistant tokens stored by this service. 
    *
    * @return {string} The ASP.NET access token.
    */
    function getToken() {
      var accessToken = parseHashAsQueryString().access_token;
      return accessToken || localStorageService.get('token');
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
				  localStorageService.set('token', data.access_token);
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
				  deferred.reject(data.error_description);
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
  | METHOD: GET USER INFO
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getUserInfo
    * @kind  function
    * @description Retrieves information on the currently authenticated user based on their access token.
    *
    * @return {promise} The user info callback promise.
    */
    function getUserInfo() {
      var deferred = $q.defer();
      $http.get(
        '/api/Account/UserInfo',
        {
          headers: {
            authorization: 'bearer ' + getToken()
          }
        }
      )
				.success(function (data, status, headers, config) {
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
				  deferred.reject(data.Message);
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
  | METHOD: REGISTER EXTERNAL
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#registerExternal
    * @kind  function
    * @description Registers a user who is authenticated via an external login provider (e.g., Facebook).
    * 
    * @param {string} email The email address to associate the user with.  
    *
    * @return {promise} The external registration callback promise.
    */
    function registerExternal(email) {
      var deferred = $q.defer();

      $http.post(
        '/api/Account/RegisterExternal',
        {
          Email: email
        },
        {
          headers: {
            authorization: 'bearer ' + getToken()
          }
        }
      )
        .success(function (registerResponse, status, headers, config) {
          deferred.resolve(registerResponse);
        })
        .error(function (errorData, status, headers, config) {
          if (errorData.ModelState) {
            deferred.reject(parseErrors(errorData));
          }
          else {
            deferred.reject(errorData.Message);
          }
        });
      return deferred.promise;
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

      var accessToken = parseHashAsQueryString().access_token;

      var userInfoPromise = getUserInfo();

      var registrationPromise = userInfoPromise.then(function(userInfo, status, headers, config) {
        if (userInfo.HasRegistered) {
          localStorageService.set('token', accessToken);
          deferred.resolve(true);
          return;
        }
        registerExternal(userInfo.Email).then(function(registerResponse, status, headers, config) {
          deferred.resolve(registerResponse);
          getLoginProviderUrl(userInfo.LoginProvider).then(function(url) {
            window.location.href = decodeURI(url);
          });
        });
      });

      registrationPromise.catch(function (data, status, headers, config) {
		    deferred.reject(data);
		  });

      return deferred.promise;

    }

  /*============================================================================================================================
  | METHOD: PARSE HASH AS QUERY STRING
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#parseHashAsQueryString
    * @kind  function
    * @description Parses the URL's based on query string conventions (e.g., `&key=value`).
    *
    * @return {object} A JavaScript object representing the key and value pairs.
    */
    function parseHashAsQueryString() {
      var hash = $location.hash();
      if (!hash || hash.indexOf('=') < 0) return {};
      return $.parseJSON('{"' + decodeURI(hash).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
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