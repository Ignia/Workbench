using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Ignia.Workbench.Models;

namespace WebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    */
    public class CommentsController : ODataController
    {
        private WorkbenchContext db = new WorkbenchContext();

        // GET: odata/Comments
        [EnableQuery]
        public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        // GET: odata/Comments(5)
        [EnableQuery]
        public SingleResult<Comment> GetComment([FromODataUri] int key)
        {
            return SingleResult.Create(db.Comments.Where(comment => comment.Id == key));
        }

        // PUT: odata/Comments(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Comment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = await db.Comments.FindAsync(key);
            if (comment == null)
            {
                return NotFound();
            }

            patch.Put(comment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(comment);
        }

        // POST: odata/Comments
        public async Task<IHttpActionResult> Post(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return Created(comment);
        }

        // PATCH: odata/Comments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Comment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = await db.Comments.FindAsync(key);
            if (comment == null)
            {
                return NotFound();
            }

            patch.Patch(comment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(comment);
        }

        // DELETE: odata/Comments(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Comment comment = await db.Comments.FindAsync(key);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Comments(5)/Post
        [EnableQuery]
        public SingleResult<Post> GetPost([FromODataUri] int key)
        {
            return SingleResult.Create(db.Comments.Where(m => m.Id == key).Select(m => m.Post));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int key)
        {
            return db.Comments.Count(e => e.Id == key) > 0;
        }
    }
}
