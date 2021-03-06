﻿/*==============================================================================================================================
| FACEBOOK INITIALIZATION 
>-------------------------------------------------------------------------------------------------------------------------------
| Attaches a global function to the Window object which will be used to initialize the Facebook control. This includes setting
| configuration variables, such as the application ID.
\-----------------------------------------------------------------------------------------------------------------------------*/
  window.fbAsyncInit = function() {

  /*----------------------------------------------------------------------------------------------------------------------------
  | Configure Facebook
  \---------------------------------------------------------------------------------------------------------------------------*/
    FB.init({
      appId      : '439510389543736',
      cookie     : true,  // enable cookies to allow the server to access the session
      xfbml      : true,  // parse social plugins on this page
      version    : 'v2.3'
    });

  /*----------------------------------------------------------------------------------------------------------------------------
  | Check login status
  >-----------------------------------------------------------------------------------------------------------------------------
  | Now that we've initialized the JavaScript SDK, we call FB.getLoginStatus(). This function gets the state of the person 
  | visiting this page and can return one of three states to the callback you provide.  They can be:
  |   1. Logged into your app ('connected')
  |   2. Logged into Facebook, but not your app ('not_authorized')
  |   3. Not logged into Facebook and can't tell if they are logged into your app or not.
  | These three cases are handled in the callback function.
  \---------------------------------------------------------------------------------------------------------------------------*/
    FB.getLoginStatus(function(response) {
      statusChangeCallback(response);
    });

  };

/*==============================================================================================================================
| IS REGISTERED?
>-------------------------------------------------------------------------------------------------------------------------------
| Checks the API to determine if the current user is registered with the local application or not.
\-----------------------------------------------------------------------------------------------------------------------------*/
  function isRegistered(fbid) {

    $.ajax({
      method: "GET",
      url: "",
      data: {  }
    })
    .done(function( msg ) {
      alert( "Data Saved: " + msg );
    });

  }

/*==============================================================================================================================
| STATUS CHANGE CALLBACK
>-------------------------------------------------------------------------------------------------------------------------------
| Called with the results from getLoginStatus(). The response object is returned with a status field that lets the app know the 
| current login status of the person. Full docs on the response object can be found in the documentation for 
| FB.getLoginStatus(). Function then updates the user interface with the status.
\-----------------------------------------------------------------------------------------------------------------------------*/
  function statusChangeCallback(response) {

  /*----------------------------------------------------------------------------------------------------------------------------
  | Write debug information
  \---------------------------------------------------------------------------------------------------------------------------*/
    console.log('statusChangeCallback');
    console.log(response);

  /*----------------------------------------------------------------------------------------------------------------------------
  | STATE: CONNECTED
  >-----------------------------------------------------------------------------------------------------------------------------
  | The user is logged into your app and Facebook.
  \---------------------------------------------------------------------------------------------------------------------------*/
    if (response.status === 'connected') {
      testAPI();
    }

  /*----------------------------------------------------------------------------------------------------------------------------
  | STATE: UNAUTHORIZED
  >-----------------------------------------------------------------------------------------------------------------------------
  | The user is logged into Facebook, but not your app.
  \---------------------------------------------------------------------------------------------------------------------------*/
    else if (response.status === 'not_authorized') {
      document.getElementById('status').innerHTML = 'Please log into this app.';
    } 

  /*----------------------------------------------------------------------------------------------------------------------------
  | STATE: UNAUTHENTICATED
  >-----------------------------------------------------------------------------------------------------------------------------
  | The person is not logged into Facebook, so we're not sure if they are logged into this app or not.
  \---------------------------------------------------------------------------------------------------------------------------*/
    else {
      document.getElementById('status').innerHTML = 'Please log into Facebook.';
    }
  }

/*==============================================================================================================================
| CHECK LOGIN STATE
>-------------------------------------------------------------------------------------------------------------------------------
| This function is called when someone finishes with the Login Button. See the onlogin handler attached to it in the sample code 
| below.
\-----------------------------------------------------------------------------------------------------------------------------*/
  function checkLoginState() {
    FB.getLoginStatus(function(response) {
      statusChangeCallback(response);
    });
  }

/*==============================================================================================================================
| TEST API
>-------------------------------------------------------------------------------------------------------------------------------
| A very simple call of the graph API to confirm that the connection is valid. A "real" app would obviously do more than this.
| This is called by the statusChangeCallback() method.
\-----------------------------------------------------------------------------------------------------------------------------*/
  function testAPI() {
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function(response) {
      console.log('Successful login for: ' + response.name);
      document.getElementById('status').innerHTML = 'Thanks for logging in, ' + response.name + '!';
    });
  }

/*==============================================================================================================================
| LOAD THE FACEBOOK SDK
>-------------------------------------------------------------------------------------------------------------------------------
| Asynchronously loads the Facebook libraries from Facebook's CDN via an IIFE.
\-----------------------------------------------------------------------------------------------------------------------------*/
  (function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
  }(document, 'script', 'facebook-jssdk'));
