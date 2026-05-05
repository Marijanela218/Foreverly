namespace Foreverly.Models
{
    public class PlaylistItem
    {
        public int Id { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;

        public int? SongId { get; set; }
        public Song? Song { get; set; }

        public int? SongGroupId { get; set; }
        public SongGroup? SongGroup { get; set; }
    }
}