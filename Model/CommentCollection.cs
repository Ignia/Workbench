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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {

  /// <summary>
  ///   Collection of comments keyed by Id.
  /// </summary>
  /// <seealso cref="T:System.Collections.ObjectModel.KeyedCollection{System.Int32,BlackCane.Mask.Comment}"/>
  public class CommentCollection : SynchronizedKeyedCollection<int, Comment> {

    /// <summary>
    ///   Initializes a new instance of the CommentCollection class.
    /// </summary>
    public CommentCollection() { }

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

    public static implicit operator CommentCollection(HashSet<User> v) {
      throw new NotImplementedException();
    }

  } //Class
} //Namespace
