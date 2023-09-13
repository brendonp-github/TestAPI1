using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestAPI1.Data;
using TestAPI1.Data.Models;

namespace TestAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public PostController(ApplicationDBContext context) => _context = context;

        // GET: api/Post
        [HttpGet]
        public async Task<Post[]> GetPost()
        {
            return await _context.Post.ToArrayAsync();
        }
        private bool PostExists(int postID)
        {
            return _context.Post.Any(e => e.PostID == postID);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int postID, Post post)
        {
            if (postID != post.PostID)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(postID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostPost(Post post)
        {
            _context.Post.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction("PostPost", new { id = post.PostID }, post);
        }

        [HttpDelete]
        public async Task deleteAll()
        {
            var posts = await _context.Post.ToArrayAsync();
            foreach (var post in posts)
            {
                _context.Post.Remove(post);
            }
            await _context.SaveChangesAsync();
        }
    }
}
