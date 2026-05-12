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
    [Route("api/v1/placila")]
    [ApiController]
    public class PlaciloApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaciloApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PlaciloApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Placilo>>> GetPlacilo()
        {
            return await _context.Placilo.ToListAsync();
        }

        // GET: api/PlaciloApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Placilo>> GetPlacilo(int id)
        {
            var placilo = await _context.Placilo.FindAsync(id);

            if (placilo == null)
            {
                return NotFound();
            }

            return placilo;
        }

        // PUT: api/PlaciloApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlacilo(int id, Placilo placilo)
        {
            if (id != placilo.Id)
            {
                return BadRequest();
            }

            _context.Entry(placilo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaciloExists(id))
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

        // POST: api/PlaciloApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Placilo>> PostPlacilo(Placilo placilo)
        {
            _context.Placilo.Add(placilo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlacilo", new { id = placilo.Id }, placilo);
        }

        // DELETE: api/PlaciloApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlacilo(int id)
        {
            var placilo = await _context.Placilo.FindAsync(id);
            if (placilo == null)
            {
                return NotFound();
            }

            _context.Placilo.Remove(placilo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaciloExists(int id)
        {
            return _context.Placilo.Any(e => e.Id == id);
        }
    }
}
