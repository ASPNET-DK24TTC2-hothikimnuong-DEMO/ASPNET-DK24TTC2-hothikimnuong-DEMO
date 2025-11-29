using System;
using System.Collections.Generic;

namespace BanVePhimOnline.Models
{
    public class Showtime
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        
        public int CinemaHallId { get; set; }
        public CinemaHall? CinemaHall { get; set; }
        
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        
        public ICollection<Booking>? Bookings { get; set; }
    }
}
