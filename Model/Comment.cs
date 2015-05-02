﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Workbench.Models {

  /// <summary>
  ///   The Comment class represents a comment on an individual post.
  /// </summary>
  public class Comment {

    /// <summary>
    ///   Initializes a new instance of the Comment class.
    /// </summary>
    public Comment() {
      DateCreated = DateTime.Now;
      Likes = new UserCollection();
    }

    /// <summary>
    ///   Gets or sets the identifier.  This property is autogenerated by the database and does not need to be set manually.
    /// </summary>
    /// <value>
    ///   The identifier.
    /// </value>
    [Column("CommentID")]
    public int Id { get; set; }

    /// <summary>
    ///   Gets or sets the body text of the comment.  A comment must be no shorter than two characters, and is currently restricted to a maximum length of 500 characters.
    /// </summary>
    /// <value>
    ///   The body.
    /// </value>
    [Required,
     MaxLength(500, ErrorMessage = "Comment body must be no more than 500 characters."),
     MinLength(2, ErrorMessage = "Comment body must be at least two characters.")
     ]
    public String Body { get; set; }

    /// <summary>
    ///   Gets or sets the Date/Time of when the comment was created.
    /// </summary>
    /// <value>
    ///   The date created.
    /// </value>
    [Required]
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///   Gets or sets the identifier of the post that this comment is in response to.
    /// </summary>
    /// <value>
    ///   The identifier of the post.
    /// </value>
    public int PostId { get; set; }

    /// <summary>
    ///   Gets or sets the reference to the post that this comment is in response to.
    /// </summary>
    /// <value>
    ///   The post.
    /// </value>
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }

    /// <summary>
    ///   Gets or sets the identifier of the user that created this comment.
    /// </summary>
    /// <value>
    ///   The identifier of the user.
    /// </value>
    public string UserId { get; set; }

    /// <summary>
    ///   Gets or sets the user that created this comment.
    /// </summary>
    /// <value>
    ///   The user.
    /// </value>
    [Required]
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    /// <summary>
    ///   Gets or sets the collection of users who liked this comment.
    /// </summary>
    /// <value>
    ///   The users who liked this comment.
    /// </value>
    [InverseProperty("LikedComments")]
    public virtual UserCollection Likes { get; set; }

    /// <summary>
    ///   Gets or sets the time stamp, which is a byte array representing the time when the object was last updated.  ADO.NET
    ///   Entity Framework uses the time stamp field to provide concurrency checking during updates.  In typical use, this is not
    ///   expected to be used by developers - and, in fact, will rarely be used by the system on account of editing objects being
    ///   an unexpected use case.
    /// </summary>
    /// <value>
    ///   The time stamp.
    /// </value>
    [IgnoreDataMember]
    [Timestamp]
    public Byte[] TimeStamp { get; set; }

  } //Class
} //Namespace