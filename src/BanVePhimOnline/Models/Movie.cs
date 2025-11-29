using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanVePhimOnline.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Genre { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string AgeRating { get; set; } // e.g., P, C13, C16, C18

        public ICollection<Showtime>? Showtimes { get; set; }
    }
}
