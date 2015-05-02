/*==============================================================================================================================
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Ignia.Workbench.Models {

  /*============================================================================================================================
  | CLASS: POST COLLECTION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Collection of posts keyed by Id.
  /// </summary>
  /// <seealso cref="T:System.Collections.ObjectModel.KeyedCollection{Int32,Post}"/>
  public class PostCollection : KeyedCollection<int, Post> {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the PostCollection class.
    /// </summary>
    public PostCollection() {
    }

    /*==========================================================================================================================
    | METHOD: GET KEY FOR ITEM
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets key for item.  This will provide a means of looking up individual items based on a friendly identifier.  Specifically, this colletion uses the Post.Id as the lookup.
    /// </summary>
    /// <param name="post">The comment.</param>
    /// <returns>
    ///   The key for item.
    /// </returns>
    protected override int GetKeyForItem(Post post) {
      return post.Id;
    }

  } //Class
} //Namespace
