namespace Foreverly.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Artist { get; set; }
        public string? Genre { get; set; }

        public ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
    }
}