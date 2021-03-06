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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {

  /*============================================================================================================================
  | CLASS: COMMENT COLLECTION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Collection of comments keyed by Id.
  /// </summary>
  /// <seealso cref="T:System.Collections.ObjectModel.KeyedCollection{Int32,Comment}"/>
  public class CommentCollection : KeyedCollection<int, Comment> {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the CommentCollection class.
    /// </summary>
    public CommentCollection() { }

    /*==========================================================================================================================
    | METHOD: GET KEY FOR ITEM
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets key for item.  This will provide a means of looking up individual items based on a friendly identifier.  Specifically, this colletion uses the Comment.Id as the lookup.
    /// </summary>
    /// <param name="comment">The comment.</param>
    /// <returns>
    ///   The key for item.
    /// </returns>
    protected override int GetKeyForItem(Comment comment) {
      return comment.Id;
    }

  } //Class
} //Namespace
