namespace Foreverly.Models
{
    public class Partner
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public PartnerCategory Category { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set; }

        public decimal DefaultCommissionPercent { get; set; }

        public string? Notes { get; set; }

        public Band? Band { get; set; }
        public Restaurant? Restaurant { get; set; }

        public ICollection<FloralArrangement> FloralArrangements { get; set; } = new List<FloralArrangement>();
        public ICollection<PastryItem> PastryItems { get; set; } = new List<PastryItem>();
        public ICollection<WeddingService> WeddingServices { get; set; } = new List<WeddingService>();
        public ICollection<PartnerBooking> PartnerBookings { get; set; } = new List<PartnerBooking>();
    }
}