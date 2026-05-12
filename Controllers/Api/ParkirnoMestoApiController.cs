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
        private readonly SmartParkContext _context;

        public ParkirnoMestoApiController(SmartParkContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkirnoMesto>>> GetParkirnoMesto()
        {
            return await _context.ParkirnaMesta.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkirnoMesto>> GetParkirnoMesto(int id)
        {
            var parkirnoMesto = await _context.ParkirnaMesta.FindAsync(id);

            if (parkirnoMesto == null)
                return NotFound();

            return parkirnoMesto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkirnoMesto(int id, ParkirnoMesto parkirnoMesto)
        {
            if (id != parkirnoMesto.Id)
                return BadRequest();

            _context.Entry(parkirnoMesto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ParkirnaMesta.AnyAsync(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ParkirnoMesto>> PostParkirnoMesto(ParkirnoMesto parkirnoMesto)
        {
            _context.ParkirnaMesta.Add(parkirnoMesto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParkirnoMesto), new { id = parkirnoMesto.Id }, parkirnoMesto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkirnoMesto(int id)
        {
            var parkirnoMesto = await _context.ParkirnaMesta.FindAsync(id);
            if (parkirnoMesto == null)
                return NotFound();

            _context.ParkirnaMesta.Remove(parkirnoMesto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}