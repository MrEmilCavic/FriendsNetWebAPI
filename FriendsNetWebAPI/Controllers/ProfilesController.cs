using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsNetWebAPI.Models;

namespace FriendsNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly FriendsNetContext _context;

        public ProfilesController(FriendsNetContext context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profiles>>> GetProfiles()
        {
            return await _context.Profiles.ToListAsync();
        }

        // GET: api/Profiles/5
        [HttpGet("{userID}")]
        public async Task<ActionResult<Profiles>> GetProfiles(int userID)
        {
            var profiles = await _context.Profiles.FindAsync(userID);

            if (profiles == null)
            {
                return NotFound();
            }

            return profiles;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userID}")]
        [Authorize]
        public async Task<IActionResult> PutProfiles(int userID, Profiles profiles)
        {
            if (userID != profiles.userID)
            {
                return BadRequest();
            }

            _context.Entry(profiles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfilesExists(userID))
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

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profiles>> PostProfiles(Profiles profiles)
        {
            _context.Profiles.Add(profiles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfiles", new { userID = profiles.userID }, profiles);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{userID}")]
        public async Task<IActionResult> DeleteProfiles(int userID)
        {
            var profiles = await _context.Profiles.FindAsync(userID);
            if (profiles == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profiles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfilesExists(int userID)
        {
            return _context.Profiles.Any(e => e.userID == userID);
        }
    }
}