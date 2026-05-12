using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;
using SmartPark.Filters;

namespace SmartPark.Controllers_Api
{
    [Route("api/v1/parkirisca")]
    [ApiController]
    [ApiKeyAuth]
    public class ParkirisceApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParkirisceApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ParkirisceApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parkirisce>>> GetParkirisce()
        {
            return await _context.Parkirisce.ToListAsync();
        }

        // GET: api/ParkirisceApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parkirisce>> GetParkirisce(int id)
        {
            var parkirisce = await _context.Parkirisce.FindAsync(id);

            if (parkirisce == null)
            {
                return NotFound();
            }

            return parkirisce;
        }

        // PUT: api/ParkirisceApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkirisce(int id, Parkirisce parkirisce)
        {
            if (id != parkirisce.Id)
            {
                return BadRequest();
            }

            _context.Entry(parkirisce).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkirisceExists(id))
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

        // POST: api/ParkirisceApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parkirisce>> PostParkirisce(Parkirisce parkirisce)
        {
            _context.Parkirisce.Add(parkirisce);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParkirisce", new { id = parkirisce.Id }, parkirisce);
        }

        // DELETE: api/ParkirisceApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkirisce(int id)
        {
            var parkirisce = await _context.Parkirisce.FindAsync(id);
            if (parkirisce == null)
            {
                return NotFound();
            }

            _context.Parkirisce.Remove(parkirisce);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParkirisceExists(int id)
        {
            return _context.Parkirisce.Any(e => e.Id == id);
        }
    }
}
