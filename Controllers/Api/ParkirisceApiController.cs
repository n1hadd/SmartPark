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
        private readonly SmartParkContext _context;

        public ParkirisceApiController(SmartParkContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parkirisce>>> GetParkirisce()
        {
            return await _context.Parkirisca.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parkirisce>> GetParkirisce(int id)
        {
            var parkirisce = await _context.Parkirisca.FindAsync(id);

            if (parkirisce == null)
                return NotFound();

            return parkirisce;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkirisce(int id, Parkirisce parkirisce)
        {
            if (id != parkirisce.Id)
                return BadRequest();

            _context.Entry(parkirisce).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Parkirisca.AnyAsync(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Parkirisce>> PostParkirisce(Parkirisce parkirisce)
        {
            _context.Parkirisca.Add(parkirisce);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParkirisce), new { id = parkirisce.Id }, parkirisce);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkirisce(int id)
        {
            var parkirisce = await _context.Parkirisca.FindAsync(id);
            if (parkirisce == null)
                return NotFound();

            _context.Parkirisca.Remove(parkirisce);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Parkirisce>>> Search(double lat, double lon, double radiusKm = 3)
        {
            // grob bounding box filter (hitro in dovolj za demo)
            var latDelta = radiusKm / 111.0;
            var lonDelta = radiusKm / (111.0 * Math.Cos(lat * Math.PI / 180.0));

            var minLat = lat - latDelta;
            var maxLat = lat + latDelta;
            var minLon = lon - lonDelta;
            var maxLon = lon + lonDelta;

            var query = _context.Parkirisca
                .Where(p => p.Latitude >= minLat && p.Latitude <= maxLat
                        && p.Longitude >= minLon && p.Longitude <= maxLon);

            return await query.ToListAsync();
        }
    }
}