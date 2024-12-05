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
    public class CredentialsController : ControllerBase
    {
        private readonly FriendsNetContext _context;

        public CredentialsController(FriendsNetContext context)
        {
            _context = context;
        }

        // GET: api/Credentials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Credentials>>> GetCredentials()
        {
            return await _context.Credentials.ToListAsync();
        }

        // GET: api/Credentials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Credentials>> GetCredentials(int id)
        {
            var credentials = await _context.Credentials.FindAsync(id);

            if (credentials == null)
            {
                return NotFound();
            }

            return credentials;
        }

        // PUT: api/Credentials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCredentials(int id, Credentials credentials)
        {
            if (id != credentials.userID)
            {
                return BadRequest();
            }

            _context.Entry(credentials).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredentialsExists(id))
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

        // POST: api/Credentials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Credentials>> PostCredentials(Credentials credentials)
        {
            _context.Credentials.Add(credentials);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCredentials", new { id = credentials.userID }, credentials);
        }

        // DELETE: api/Credentials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredentials(int id)
        {
            var credentials = await _context.Credentials.FindAsync(id);
            if (credentials == null)
            {
                return NotFound();
            }

            _context.Credentials.Remove(credentials);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CredentialsExists(int id)
        {
            return _context.Credentials.Any(e => e.userID == id);
        }
    }
}