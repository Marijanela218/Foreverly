namespace Foreverly.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}