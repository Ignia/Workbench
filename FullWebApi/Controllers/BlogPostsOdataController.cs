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
using FullWebApi.Models;

namespace FullWebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using FullWebApi.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<BlogPost>("BlogPostsOdata");
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BlogPostsOdataController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/BlogPostsOdata
        [EnableQuery]
        public IQueryable<BlogPost> GetBlogPostsOdata()
        {
            return db.BlogPosts;
        }

        // GET: odata/BlogPostsOdata(5)
        [EnableQuery]
        public SingleResult<BlogPost> GetBlogPost([FromODataUri] int key)
        {
            return SingleResult.Create(db.BlogPosts.Where(blogPost => blogPost.Id == key));
        }

        // PUT: odata/BlogPostsOdata(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<BlogPost> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BlogPost blogPost = await db.BlogPosts.FindAsync(key);
            if (blogPost == null)
            {
                return NotFound();
            }

            patch.Put(blogPost);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(blogPost);
        }

        // POST: odata/BlogPostsOdata
        public async Task<IHttpActionResult> Post(BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BlogPosts.Add(blogPost);
            await db.SaveChangesAsync();

            return Created(blogPost);
        }

        // PATCH: odata/BlogPostsOdata(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<BlogPost> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BlogPost blogPost = await db.BlogPosts.FindAsync(key);
            if (blogPost == null)
            {
                return NotFound();
            }

            patch.Patch(blogPost);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(blogPost);
        }

        // DELETE: odata/BlogPostsOdata(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            BlogPost blogPost = await db.BlogPosts.FindAsync(key);
            if (blogPost == null)
            {
                return NotFound();
            }

            db.BlogPosts.Remove(blogPost);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/BlogPostsOdata(5)/User
        [EnableQuery]
        public SingleResult<ApplicationUser> GetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.BlogPosts.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlogPostExists(int key)
        {
            return db.BlogPosts.Count(e => e.Id == key) > 0;
        }
    }
}
