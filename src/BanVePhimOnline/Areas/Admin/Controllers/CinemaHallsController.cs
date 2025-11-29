using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanVePhimOnline.Data;
using BanVePhimOnline.Models;
using Microsoft.AspNetCore.Authorization;

namespace BanVePhimOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CinemaHallsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemaHallsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CinemaHalls.Include(c => c.Cinema);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CinemaId,TotalSeats")] CinemaHall cinemaHall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinemaHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", cinemaHall.CinemaId);
            return View(cinemaHall);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cinemaHall = await _context.CinemaHalls.FindAsync(id);
            if (cinemaHall == null) return NotFound();
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", cinemaHall.CinemaId);
            return View(cinemaHall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CinemaId,TotalSeats")] CinemaHall cinemaHall)
        {
            if (id != cinemaHall.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinemaHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CinemaHalls.Any(e => e.Id == cinemaHall.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", cinemaHall.CinemaId);
            return View(cinemaHall);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cinemaHall = await _context.CinemaHalls
                .Include(c => c.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemaHall == null) return NotFound();

            return View(cinemaHall);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaHall = await _context.CinemaHalls.FindAsync(id);
            if (cinemaHall != null) _context.CinemaHalls.Remove(cinemaHall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
