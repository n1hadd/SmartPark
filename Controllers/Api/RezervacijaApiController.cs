using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;
using SmartPark.Filters;

namespace SmartPark.Controllers_Api
{
    [Route("api/v1/rezervacije")]
    [ApiController]
    [ApiKeyAuthAttribute]
    public class RezervacijaApiController : ControllerBase
    {
        private readonly SmartParkContext _context;

        public RezervacijaApiController(SmartParkContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rezervacija>>> GetRezervacija()
        {
            return await _context.Rezervacije.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rezervacija>> GetRezervacija(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
                return NotFound();

            return rezervacija;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervacija(int id, Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
                return BadRequest();

            _context.Entry(rezervacija).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Rezervacije.AnyAsync(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Rezervacija>> PostRezervacija(Rezervacija rezervacija)
        {
            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRezervacija), new { id = rezervacija.Id }, rezervacija);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervacija(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
                return NotFound();

            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}