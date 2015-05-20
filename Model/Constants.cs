/*==============================================================================================================================
| Author        Jeremy Caney (Jeremy.Caney@Ignia.com)
| Client        Ignia
| Project       Workbench
>===============================================================================================================================
| Revisions     Date        Author              Comments
|- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|               20.05.15    Jeremy Caney        Created initial version.
\=============================================================================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {

  /*============================================================================================================================
  | CLASS: CONSTANTS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides read only access to constants used by the model.
  /// </summary>
  /// <remarks>
  ///   This file is established a placeholder where constants potentially required by multiple areas of the application can be
  ///   stored.
  /// </remarks>
  public static class Constants {

    /*==========================================================================================================================
    | FACEBOOK APP ID
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Facebook Application ID.
    /// </summary>
    /// <value>The Facebook Application ID.</value>
    public static string FacebookAppId {
      get {
        return "";
      }
    }

    /*==========================================================================================================================
    | FACEBOOK APPLICATION SECRET
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Facebook Application Secret.
    /// </summary>
    /// <value>The Facebook Application Secret.</value>
    public static string FacebookAppSecret {
      get {
        return "";
      }
    }

    /*==========================================================================================================================
    | TWITTER CONSUMER KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Twitter Consumer Key.
    /// </summary>
    /// <value>The Twitter Consumer Key.</value>
    public static string TwitterConsumerKey {
      get {
        return "";
      }
    }

    /*==========================================================================================================================
    | TWITTER CONSUMER SECRET
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Twitter Consumer Secret.
    /// </summary>
    /// <value>The Twitter Consumer Secret.</value>
    public static string TwitterConsumerSecret {
      get {
        return "";
      }
    }

    /*==========================================================================================================================
    | GOOGLE CLIENT ID
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Google Client ID.
    /// </summary>
    /// <value>The Google Client ID.</value>
    public static string GoogleClientId {
      get {
        return "";
      }
    }

    /*==========================================================================================================================
    | GOOGLE CLIENT SECRET
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Google Client Secret.
    /// </summary>
    /// <value>The Google Client Secret.</value>
    public static string GoogleClientSecret {
      get {
        return "";
      }
    }

    /*==========================================================================================================================
    | MICROSOFT CLIENT ID
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Microsoft Client ID.
    /// </summary>
    /// <value>The Microsoft Client ID.</value>
    public static long MicrosoftClientId {
      get {
        return -1;
      }
    }

    /*==========================================================================================================================
    | MICROSOFT CLIENT SECRET
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The Microsoft Client Secret.
    /// </summary>
    /// <value>The Microsoft Client Secret.</value>
    public static string MicrosoftClientSecret {
      get {
        return "";
      }
    }

  } //Class
} //Namespace
