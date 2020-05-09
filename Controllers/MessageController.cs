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
    public class MessageController : ControllerBase
    {
        private readonly Db _db;

        public MessageController(Db db)
        {
            _db = db;
        }

        // GET: api/Message
        [EnableCors("CustomPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _db.Messages.ToListAsync();
        }

        // GET: api/Message/5
        [EnableCors("CustomPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _db.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Message/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [EnableCors("CustomPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _db.Entry(message).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Message
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [EnableCors("CustomPolicy")]
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Message/5
        [EnableCors("CustomPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteMessage(int id)
        {
            var message = await _db.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();

            return Ok(message);
        }

        private bool MessageExists(int id)
        {
            return _db.Messages.Any(e => e.Id == id);
        }
    }
}
