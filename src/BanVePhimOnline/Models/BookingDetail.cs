namespace BanVePhimOnline.Models
{
    public class BookingDetail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        
        public string SeatNumber { get; set; } // e.g., A1, B2
        public decimal Price { get; set; }
    }
}
