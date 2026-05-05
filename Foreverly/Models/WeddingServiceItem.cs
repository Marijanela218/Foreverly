namespace Foreverly.Models
{
    public class WeddingServiceItem
    {
        public int Id { get; set; }

        public int WeddingServiceId { get; set; }
        public WeddingService WeddingService { get; set; } = null!;

        public string ItemReferenceType { get; set; } = string.Empty;
        public int? ItemReferenceId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
} 