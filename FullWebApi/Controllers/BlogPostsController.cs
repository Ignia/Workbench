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
using System.Web.Http.Description;
using FullWebApi.Models;

namespace FullWebApi.Controllers
{
    public class BlogPostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BlogPosts
        public IQueryable<BlogPost> GetBlogPosts()
        {
            return db.BlogPosts;
        }

        // GET: api/BlogPosts/5
        [ResponseType(typeof(BlogPost))]
        public async Task<IHttpActionResult> GetBlogPost(int id)
        {
            BlogPost blogPost = await db.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        // PUT: api/BlogPosts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBlogPost(int id, BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blogPost.Id)
            {
                return BadRequest();
            }

            db.Entry(blogPost).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BlogPosts
        [ResponseType(typeof(BlogPost))]
        public async Task<IHttpActionResult> PostBlogPost(BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BlogPosts.Add(blogPost);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = blogPost.Id }, blogPost);
        }

        // DELETE: api/BlogPosts/5
        [ResponseType(typeof(BlogPost))]
        public async Task<IHttpActionResult> DeleteBlogPost(int id)
        {
            BlogPost blogPost = await db.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            db.BlogPosts.Remove(blogPost);
            await db.SaveChangesAsync();

            return Ok(blogPost);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlogPostExists(int id)
        {
            return db.BlogPosts.Count(e => e.Id == id) > 0;
        }
    }
}