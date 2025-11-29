using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanVePhimOnline.Data;
using BanVePhimOnline.Models;
using System.Security.Claims;

namespace BanVePhimOnline.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SelectSeats(int showtimeId)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .ThenInclude(ch => ch.Cinema)
                .Include(s => s.Bookings)
                .ThenInclude(b => b.BookingDetails)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);

            if (showtime == null) return NotFound();

            // Get booked seats
            var bookedSeats = showtime.Bookings
                .SelectMany(b => b.BookingDetails)
                .Select(bd => bd.SeatNumber)
                .ToList();

            ViewBag.BookedSeats = bookedSeats;

            return View(showtime);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int showtimeId, string selectedSeats)
        {
            if (string.IsNullOrEmpty(selectedSeats))
            {
                return RedirectToAction("SelectSeats", new { showtimeId });
            }

            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .ThenInclude(ch => ch.Cinema)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);

            if (showtime == null) return NotFound();

            var seats = selectedSeats.Split(',');
            var totalAmount = seats.Length * showtime.Price;

            var booking = new Booking
            {
                ShowtimeId = showtimeId,
                Showtime = showtime,
                BookingDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",
                BookingDetails = new List<BookingDetail>()
            };

            foreach (var seat in seats)
            {
                booking.BookingDetails.Add(new BookingDetail
                {
                    SeatNumber = seat,
                    Price = showtime.Price
                });
            }
            
            // Pass booking object to view for confirmation (not saving yet or saving as pending)
            // For simplicity, let's save as Pending and go to payment/confirmation
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            booking.UserId = userId;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { bookingId = booking.Id });
        }

        public async Task<IActionResult> Confirmation(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.CinemaHall)
                .ThenInclude(ch => ch.Cinema)
                .Include(b => b.BookingDetails)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int bookingId)
        {
             var booking = await _context.Bookings.FindAsync(bookingId);
             if (booking == null || booking.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
             {
                 return NotFound();
             }

             booking.Status = "Confirmed";
             await _context.SaveChangesAsync();

             return View("Success", booking);
        }
    }
}
