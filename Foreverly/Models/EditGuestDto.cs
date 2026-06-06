namespace Foreverly.Models
{
    public class EditGuestDto
    {
        public int GuestId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Side { get; set; } = string.Empty;
    }
}