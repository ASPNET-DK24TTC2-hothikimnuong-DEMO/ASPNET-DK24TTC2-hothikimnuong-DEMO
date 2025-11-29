using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanVePhimOnline.Models
{
    public class CinemaHall
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } // e.g., Hall 1, IMAX
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        
        public int TotalSeats { get; set; }
        
        public ICollection<Showtime>? Showtimes { get; set; }
    }
}
