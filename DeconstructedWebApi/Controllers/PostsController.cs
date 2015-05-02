using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Ignia.Workbench.Models;

namespace Ignia.Workbench.DeconstructedWebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Ignia.Workbench.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Post>("Posts");
    builder.EntitySet<Comment>("Comments"); 
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PostsController : ODataController
    {
        private WorkbenchContext db = new WorkbenchContext();

        // GET: odata/Posts
        [EnableQuery]
        public IQueryable<Post> GetPosts()
        {
            return db.Posts;
        }

        // GET: odata/Posts(5)
        [EnableQuery]
        public SingleResult<Post> GetPost([FromODataUri] int key)
        {
            return SingleResult.Create(db.Posts.Where(post => post.Id == key));
        }

        // PUT: odata/Posts(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Post> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = db.Posts.Find(key);
            if (post == null)
            {
                return NotFound();
            }

            patch.Put(post);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(post);
        }

        // POST: odata/Posts
        public IHttpActionResult Post(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(post);
            db.SaveChanges();

            return Created(post);
        }

        // PATCH: odata/Posts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Post> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = db.Posts.Find(key);
            if (post == null)
            {
                return NotFound();
            }

            patch.Patch(post);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(post);
        }

        // DELETE: odata/Posts(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Post post = db.Posts.Find(key);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Posts(5)/Comments
        [EnableQuery]
        public IQueryable<Comment> GetComments([FromODataUri] int key)
        {
            return db.Posts.Where(m => m.Id == key).SelectMany(m => m.Comments);
        }

        // GET: odata/Posts(5)/Likes
        [EnableQuery]
        public IQueryable<User> GetLikes([FromODataUri] int key)
        {
            return db.Posts.Where(m => m.Id == key).SelectMany(m => m.Likes);
        }

        // GET: odata/Posts(5)/TaggedUsers
        [EnableQuery]
        public IQueryable<User> GetTaggedUsers([FromODataUri] int key)
        {
            return db.Posts.Where(m => m.Id == key).SelectMany(m => m.TaggedUsers);
        }

        // GET: odata/Posts(5)/User
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.Posts.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int key)
        {
            return db.Posts.Count(e => e.Id == key) > 0;
        }
    }
}
