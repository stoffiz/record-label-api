using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordLabelApi.Data;

namespace RecordLabelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly Db _db;

        public NewsController(Db db)
        {
            _db = db;
        }

        // GET: api/News
        [EnableCors("CustomPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            return await _db.News.OrderBy(n => n.Published).ToListAsync();
        }

        // GET: api/News/5
        [EnableCors("CustomPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            var news = await _db.News.FindAsync(id);

            if (news == null)
            {
                return NotFound(new { message = string.Format("Could not find news post with id: {0}", id) });
            }

            return news;
        }

        // PUT: api/News/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [EnableCors("CustomPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, News news)
        {
            var newsPost = await _db.News.FirstOrDefaultAsync(n => n.Id == id);
            if (newsPost == null)
            {
                return BadRequest();
            }

            newsPost.Title = news.Title;
            newsPost.Body = news.Body;
            newsPost.Author = news.Author;
            newsPost.Published = news.Published;
            newsPost.Updated = news.Updated;

            await _db.SaveChangesAsync();
            
            return Ok(news);
        }

        // POST: api/News
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [EnableCors("CustomPolicy")]
        [HttpPost]
        public async Task<ActionResult<News>> PostNews(News news)
        {
            _db.News.Add(news);
            await _db.SaveChangesAsync();

            return Ok(new { message = string.Format("{0} - {1} was added to the database", news.Title, news.Author), news });
        }

        // DELETE: api/News/5
        [EnableCors("CustomPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            var news = await _db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound(new { message = string.Format("Could not delete news post with id: {0}", news.Id) });
            }

            _db.News.Remove(news);
            await _db.SaveChangesAsync();

            return Ok();
        }

        private bool NewsExists(int id)
        {
            return _db.News.Any(e => e.Id == id);
        }
    }
}
