using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsNetWebAPI.Models;

namespace FriendsNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventDiscussionsController : ControllerBase
    {
        private readonly FriendsNetContext _context;

        public EventDiscussionsController(FriendsNetContext context)
        {
            _context = context;
        }

        // GET: api/EventDiscussions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDiscussion>>> GetEventDiscussion()
        {
            return await _context.EventDiscussion.ToListAsync();
        }

        // GET: api/EventDiscussions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDiscussion>> GetEventDiscussion(int id)
        {
            var eventDiscussion = await _context.EventDiscussion.FindAsync(id);

            if (eventDiscussion == null)
            {
                return NotFound();
            }

            return eventDiscussion;
        }

        // PUT: api/EventDiscussions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventDiscussion(int id, EventDiscussion eventDiscussion)
        {
            if (id != eventDiscussion.eventsID)
            {
                return BadRequest();
            }

            _context.Entry(eventDiscussion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventDiscussionExists(id))
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

        // POST: api/EventDiscussions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventDiscussion>> PostEventDiscussion(EventDiscussion eventDiscussion)
        {
            _context.EventDiscussion.Add(eventDiscussion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventDiscussion", new { id = eventDiscussion.eventsID }, eventDiscussion);
        }

        // DELETE: api/EventDiscussions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventDiscussion(int id)
        {
            var eventDiscussion = await _context.EventDiscussion.FindAsync(id);
            if (eventDiscussion == null)
            {
                return NotFound();
            }

            _context.EventDiscussion.Remove(eventDiscussion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventDiscussionExists(int id)
        {
            return _context.EventDiscussion.Any(e => e.eventsID == id);
        }
    }
}