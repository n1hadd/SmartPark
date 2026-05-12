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
    [Route("api/v1/parkirnaMesta")]
    [ApiController]
    [ApiKeyAuth]
    public class ParkirnoMestoApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParkirnoMestoApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ParkirnoMestoApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkirnoMesto>>> GetParkirnoMesto()
        {
            return await _context.ParkirnoMesto.ToListAsync();
        }

        // GET: api/ParkirnoMestoApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParkirnoMesto>> GetParkirnoMesto(int id)
        {
            var parkirnoMesto = await _context.ParkirnoMesto.FindAsync(id);

            if (parkirnoMesto == null)
            {
                return NotFound();
            }

            return parkirnoMesto;
        }

        // PUT: api/ParkirnoMestoApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkirnoMesto(int id, ParkirnoMesto parkirnoMesto)
        {
            if (id != parkirnoMesto.Id)
            {
                return BadRequest();
            }

            _context.Entry(parkirnoMesto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkirnoMestoExists(id))
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

        // POST: api/ParkirnoMestoApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParkirnoMesto>> PostParkirnoMesto(ParkirnoMesto parkirnoMesto)
        {
            _context.ParkirnoMesto.Add(parkirnoMesto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParkirnoMesto", new { id = parkirnoMesto.Id }, parkirnoMesto);
        }

        // DELETE: api/ParkirnoMestoApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkirnoMesto(int id)
        {
            var parkirnoMesto = await _context.ParkirnoMesto.FindAsync(id);
            if (parkirnoMesto == null)
            {
                return NotFound();
            }

            _context.ParkirnoMesto.Remove(parkirnoMesto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParkirnoMestoExists(int id)
        {
            return _context.ParkirnoMesto.Any(e => e.Id == id);
        }
    }
}
