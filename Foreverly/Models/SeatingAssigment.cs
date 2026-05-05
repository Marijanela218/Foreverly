namespace Foreverly.Models
{
    public class SeatingAssignment
    {
        public int Id { get; set; }

        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;

        public int TableId { get; set; }
        public WeddingTable Table { get; set; } = null!;

        public int SeatNumber { get; set; }
    }
}