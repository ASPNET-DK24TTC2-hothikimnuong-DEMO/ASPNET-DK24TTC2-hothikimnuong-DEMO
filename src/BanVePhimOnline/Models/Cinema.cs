using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanVePhimOnline.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        
        public ICollection<CinemaHall>? CinemaHalls { get; set; }
    }
}
