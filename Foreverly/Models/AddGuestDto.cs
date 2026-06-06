namespace Foreverly.Models
{
    public class AddGuestDto
    {
        public int WeddingId { get; set; }

        public string FullName { get; set; } = string.Empty;
    }
}