using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;

namespace SmartPark.Controllers.Admin
{
    [Authorize(Roles = "Administrator")]
    [Route("admin/parkirisca")]
    public class ParkiriscaAdminController : Controller
    {
        private readonly SmartParkContext _context;

        public ParkiriscaAdminController(SmartParkContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Parkirisca.OrderBy(p => p.Naslov).ToListAsync();
            return View(list);
        }

        [HttpGet("create")]
        public IActionResult Create() => View(new Parkirisce());

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Parkirisce model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Parkirisca.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _context.Parkirisca.FindAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Parkirisce model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Parkirisca.FindAsync(id);
            if (p == null) return NotFound();

            _context.Parkirisca.Remove(p);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}