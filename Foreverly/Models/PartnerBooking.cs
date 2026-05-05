namespace Foreverly.Models
{
    public class PartnerBooking
    {
        public int Id { get; set; }

        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; } = null!;

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}