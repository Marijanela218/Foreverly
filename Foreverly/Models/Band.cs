namespace Foreverly.Models
{
    public class Band
    {
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<BandPrice> BandPrices { get; set; } = new List<BandPrice>();
    }
}