namespace Foreverly.Models
{
    public class Hall
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public string? Address { get; set; }
        public decimal BasePrice { get; set; }
    }
}