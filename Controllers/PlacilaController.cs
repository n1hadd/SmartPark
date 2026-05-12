using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;

namespace SmartPark.Controllers
{
    public class PlacilaController : Controller
    {
        private readonly SmartParkContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlacilaController(SmartParkContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlaciloRequest request)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Parkirisce)
                .FirstOrDefaultAsync(r => r.Id == request.rezervacijaId && r.ApplicationUserId == userId);

            if (rezervacija == null)
                return NotFound(new { success = false, message = "Rezervacija ne obstaja ali ne pripada uporabniku." });

            var trajanjeUr = (rezervacija.Konec - rezervacija.Zacetek).TotalHours;
            var cenaNaUro = rezervacija.Parkirisce?.CenaNaUro ?? 2.0m;
            var znesek = (decimal)trajanjeUr * cenaNaUro;

            var placilo = new Placilo
            {
                RezervacijaId = rezervacija.Id,
                ApplicationUserId = userId,
                Znesek = znesek,
                NacinPlacila = request.nacinPlacila ?? "Kartica",
                Datum = DateTime.Now
            };

            _context.Placila.Add(placilo);
            await _context.SaveChangesAsync();

            return Json(new { success = true, rezervacijaId = rezervacija.Id, znesek });
        }
    }

    public class PlaciloRequest
    {
        public int rezervacijaId { get; set; }
        public string? nacinPlacila { get; set; }
    }
}
