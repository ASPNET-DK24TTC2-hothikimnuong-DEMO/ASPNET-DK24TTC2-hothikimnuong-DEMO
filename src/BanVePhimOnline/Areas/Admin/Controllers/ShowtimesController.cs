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
    public class ShowtimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowtimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Showtimes.Include(s => s.CinemaHall).ThenInclude(ch => ch.Cinema).Include(s => s.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls.Include(c => c.Cinema).Select(c => new { Id = c.Id, Name = c.Cinema.Name + " - " + c.Name }), "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,CinemaHallId,StartTime,Price")] Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(showtime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls.Include(c => c.Cinema).Select(c => new { Id = c.Id, Name = c.Cinema.Name + " - " + c.Name }), "Id", "Name", showtime.CinemaHallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", showtime.MovieId);
            return View(showtime);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null) return NotFound();
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls.Include(c => c.Cinema).Select(c => new { Id = c.Id, Name = c.Cinema.Name + " - " + c.Name }), "Id", "Name", showtime.CinemaHallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", showtime.MovieId);
            return View(showtime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,CinemaHallId,StartTime,Price")] Showtime showtime)
        {
            if (id != showtime.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showtime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Showtimes.Any(e => e.Id == showtime.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls.Include(c => c.Cinema).Select(c => new { Id = c.Id, Name = c.Cinema.Name + " - " + c.Name }), "Id", "Name", showtime.CinemaHallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", showtime.MovieId);
            return View(showtime);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var showtime = await _context.Showtimes
                .Include(s => s.CinemaHall)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showtime == null) return NotFound();

            return View(showtime);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime != null) _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
