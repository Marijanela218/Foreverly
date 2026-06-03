namespace Foreverly.Models
{
    public class Guest
    {
        public int Id { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; } = null!;

        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string Side { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}