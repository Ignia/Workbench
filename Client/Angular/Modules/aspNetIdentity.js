(function() {
  'use strict';

/*==============================================================================================================================
| Author        Jeremy Caney (Jeremy.Caney@Ignia.com)
| Client        Ignia
| Project       ASP.NET Identity Provider
>===============================================================================================================================
| Revisions     Date        Author              Comments
|- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|               21.05.15    Jeremy Caney        Created initial version.
>===============================================================================================================================
| ### TODO JJC052615: Centralize post and get logic into one or two helper functions.
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
      manageInfo                : manageInfo                    ,
      login                     : login                         ,
      loginExternal             : loginExternal                 ,
      register                  : register                      ,
      registerExternal          : registerExternal              ,
      addExternalLogin          : addExternalLogin              ,
      removeLogin               : removeLogin                   ,
      changePassword            : changePassword                ,
      setPassword               : setPassword                   ,
      getExternalLogins         : getExternalLogins             ,
      getExternalLoginUrl       : getExternalLoginUrl           ,
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
  | METHOD: CHANGE PASSWORD
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#ChangePassword
    * @kind  function
    * @description Changes the password of the currently authenticated user.
    *
    * @param {object=} options An object containing an `OldPassword` , `NewPassword`, and `ConfirmPassword` properties.  
    *
    * @return {promise} The change password callback promise.
    */
    function changePassword(options) {
      var deferred = $q.defer();
      $http.post('/API/Account/ChangePassword', options)
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
  | METHOD: SET PASSWORD
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#SetPassword
    * @kind  function
    * @description Sets the password of the currently authenticated user. This is useful for users who originally registered via 
    * an external account, and now wish to establish a password for that account. This method is not suitable for users who 
    * already have a password; in those scenarios, use the {@link aspNetIdentity#changePassword changePassword} method instead. 
    *
    * @param {object=} options An object containing a `NewPassword` and `ConfirmPassword` properties.  
    *
    * @return {promise} The set password callback promise.
    */
    function setPassword(options) {
      var deferred = $q.defer();
      $http.post('/API/Account/SetPassword', options)
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
  | METHOD: ADD EXTERNAL LOGIN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#addExternalLogin
    * @kind  function
    * @description Associates an external account (e.g., Facebook) with a current user, thus allowing them to authenticate using
    * additional accounts. When the user is registered using {@link aspNetIdentity#registerExternal registerExternal} this is 
    * automatically performed for that provider. 
    *
    * @param {object=} options An object containing a single `ExternalAccessToken` property.  
    *
    * @return {promise} The add external login callback promise.
    */
    function addExternalLogin(options) {
      var deferred = $q.defer();
      $http.post('/API/Account/AddExternalLogin', options)
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
  | METHOD: REMOVE LOGIN
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#removeLogin
    * @kind  function
    * @description Removes an external authentication method (e.g., `Facebook`) from a user's account. This does not remove the 
    * account itself, only the ability for the user to authenticate given the specified provider. 
    *
    * @param {object=} options An object containing a `LoginProvider` and `ProviderKey` property.  
    *
    * @return {promise} The remove login callback promise.
    */
    function removeLogin(options) {
      var deferred = $q.defer();
      $http.post('/API/Account/RemoveLogin', options)
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
  | METHOD: MANAGE INFO
  >-----------------------------------------------------------------------------------------------------------------------------
  | ### TODO JJC052615: Need to add code to automatically set default for ReturnUrl. Should do something similar for 
  | getExternalLogins() method.
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#manageInfo
    * @kind  function
    * @description Retrieves a list of authentication options associated with the current user account.
    *
    * @param {string} returnUrl The URL that users should be redirected to if an external login is performed. No redirect is 
    * performed as part of this method, but the parameter is used to assemble the correct URL for any external logins (e.g., 
    * `Facebook`). The default is the current URL.
    * @param {string} generateState Determines whether any external login URLs retrieved for the current users should include
    * state information or not. The default is false.  
    *
    * @return {promise} The manage info callback promise.
    */
    function manageInfo(returnUrl, generateState) {
      var deferred = $q.defer();
      $http.get('/API/Account/ManageInfo?returnUrl=' + encodeURI(returnUrl) + '&generateState=' + (generateState || false))
				.success(function (data, status, headers, config) {
				  deferred.resolve(data);
				})
				.error(function (data, status, headers, config) {
  		    deferred.reject(data.Message);
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
  | METHOD: GET EXTERNAL LOGINS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getExternalLogins
    * @kind  function
    * @description Retrieves a list of available third-party login providers, such as Facebook or Twitter, which the application 
    * is configured to accept. Each contains a name and a URL, which should be presented to the user as a list of login links. 
    * The results are cached locally.
    *
    * @return {promise} The login provider's callback promise.
    */
    function getExternalLogins() {
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
  | METHOD: GET EXTERNAL LOGIN URL
  \---------------------------------------------------------------------------------------------------------------------------*/
  /** @ngdoc method
    * @name  aspNetIdentity#getExternalLoginUrl
    * @kind  function
    * @description Given a login provider name (e.g., `Facebook`) will lookup the provider via the 
    * {@link aspNetIdentity#getExternalLogins getExternalLogins} method.
    *
    * @return {string} The URL of the login provider, if found; otherwise `null`.
    */
    function getExternalLoginUrl(name) {
      return getExternalLogins().then(function (data) {
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
          getExternalLoginUrl(userInfo.LoginProvider).then(function(url) {
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