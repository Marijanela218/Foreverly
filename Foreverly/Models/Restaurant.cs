namespace Foreverly.Models
{
    public class Restaurant
    {
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public bool HasWeddingHall { get; set; }
        public bool OffersCatering { get; set; }

        public ICollection<Hall> Halls { get; set; } = new List<Hall>();
        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
    }
}