using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tm_server.Models;

namespace tm_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly TMContext _context;

        public ClubsController(TMContext context)
        {
            _context = context;
        }

        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs(
            [FromQuery(Name = "countryId")] long CountryId,
            [FromQuery(Name = "name")] string Name
        )
        {
            if (CountryId != 0) // GET: api/Clubs?countryId=123
                return await _context.Clubs.Where<Club>(club => club.CountryId == CountryId).ToListAsync();
            if (Name != null && Name.Length > 0)
            {
                //Regex pattern = new Regex(Name);
                //return await _context.Clubs.Where<Club>(club => pattern.IsMatch(Name)).ToListAsync();
                return await _context.Clubs.Where<Club>(club => club.Name.Contains(Name)).ToListAsync();
            }
            return await _context.Clubs.ToListAsync();
        }

        // GET: api/Clubs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> GetClub(long id)
        {
            var club = await _context.Clubs.FindAsync(id);

            if (club == null)
            {
                return NotFound();
            }

            return club;
        }

        // PUT: api/Clubs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(long id, Club club)
        {
            if (id != club.Id)
            {
                return BadRequest();
            }

            _context.Entry(club).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(id))
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

        // POST: api/Clubs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Club>> PostClub(Club club)
        {
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClub", new { id = club.Id }, club);
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(long id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClubExists(long id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }
    }
}
