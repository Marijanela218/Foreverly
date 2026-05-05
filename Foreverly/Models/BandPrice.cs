namespace Foreverly.Models
{
    public class BandPrice
    {
        public int Id { get; set; }

        public int BandId { get; set; }
        public Band Band { get; set; } = null!;

        public string DayOfWeek { get; set; } = string.Empty;
        public int DurationHours { get; set; }
        public decimal Price { get; set; }
    }
}