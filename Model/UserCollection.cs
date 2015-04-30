﻿/*==============================================================================================================================
| Author        Jeremy Caney (Jeremy.Caney@Ignia.com)
| Client        Ignia
| Project       Workbench
>===============================================================================================================================
| Revisions     Date        Author              Comments
|- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|               29.04.15    Jeremy Caney        Created initial version.
\=============================================================================================================================*/

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {

  /// <summary>
  ///   Collection of users keyed by Id.
  /// </summary>
  /// <seealso cref="T:System.Collections.ObjectModel.KeyedCollection{String,User}"/>
  public class UserCollection : Collection<User> {

    /// <summary>
    ///   Initializes a new instance of the UserCollection class.
    /// </summary>
    public UserCollection() { }

    /// <summary>
    ///   Gets key for item.  This will provide a means of looking up individual items based on a friendly identifier.  Specifically, this colletion uses the User.Id as the lookup.
    /// </summary>
    /// <param name="user">The comment.</param>
    /// <returns>
    ///   The key for item.
    /// </returns>
    //protected override string GetKeyForItem(User user) {
    //  return user.Id;
    //  }
    }
  }
