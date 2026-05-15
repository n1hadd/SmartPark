using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;

namespace SmartPark.Controllers.Admin
{
    [Authorize(Roles = "Administrator")]
    [Route("admin/rezervacije")]
    public class RezervacijeAdminController : Controller
    {
        private readonly SmartParkContext _context;

        public RezervacijeAdminController(SmartParkContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var rezervacije = await _context.Rezervacije
                .Include(r => r.Uporabnik)     // <-- IMPORTANT (correct nav property)
                .Include(r => r.Parkirisce)
                .Include(r => r.ParkirnoMesto)
                .OrderByDescending(r => r.Zacetek)
                .AsNoTracking()
                .ToListAsync();

            return View(rezervacije);
        }
    }
}