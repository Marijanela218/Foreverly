namespace Foreverly.Models
{
    public class SongGroup
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
    }
}