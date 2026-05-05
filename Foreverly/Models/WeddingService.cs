namespace Foreverly.Models
{
    public class WeddingService
    {
        public int Id { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; } = null!;

        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public string ServiceType { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal CommissionPercent { get; set; }
        public decimal CommissionAmount { get; set; }

        public bool Confirmed { get; set; }

        public string? SpecialRequest { get; set; }
        public string? Notes { get; set; }

        public ICollection<WeddingServiceItem> WeddingServiceItems { get; set; } = new List<WeddingServiceItem>();
    }
}