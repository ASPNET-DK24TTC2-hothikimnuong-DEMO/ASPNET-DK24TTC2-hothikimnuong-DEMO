using System;
using System.Collections.Generic;

namespace BanVePhimOnline.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int ShowtimeId { get; set; }
        public Showtime Showtime { get; set; }
        
        public DateTime BookingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Cancelled
        
        public ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
