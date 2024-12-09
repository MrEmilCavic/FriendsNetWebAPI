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
    public class InterestProfilesController : ControllerBase
    {
        private readonly FriendsNetContext _context;

        public InterestProfilesController(FriendsNetContext context)
        {
            _context = context;
        }

        // GET: api/InterestProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InterestProfiles>>> GetInterestProfiles()
        {
            return await _context.InterestProfiles.ToListAsync();
        }

        // GET: api/InterestProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InterestProfiles>> GetInterestProfiles(int id)
        {
            var interestProfiles = await _context.InterestProfiles.FindAsync(id);

            if (interestProfiles == null)
            {
                return NotFound();
            }

            return interestProfiles;
        }

        // PUT: api/InterestProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterestProfiles(int id, InterestProfiles interestProfiles)
        {
            if (id != interestProfiles.UserID)
            {
                return BadRequest();
            }

            _context.Entry(interestProfiles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterestProfilesExists(id))
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

        // POST: api/InterestProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InterestProfiles>> PostInterestProfiles(InterestProfiles interestProfiles)
        {
            _context.InterestProfiles.Add(interestProfiles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInterestProfiles", new { id = interestProfiles.UserID }, interestProfiles);
        }

        // DELETE: api/InterestProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInterestProfiles(int id)
        {
            var interestProfiles = await _context.InterestProfiles.FindAsync(id);
            if (interestProfiles == null)
            {
                return NotFound();
            }

            _context.InterestProfiles.Remove(interestProfiles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InterestProfilesExists(int id)
        {
            return _context.InterestProfiles.Any(e => e.UserID == id);
        }
    }
}
