namespace Foreverly.Models
{
    public class Template
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<TemplateItem> TemplateItems { get; set; } = new List<TemplateItem>();
        public ICollection<Wedding> Weddings { get; set; } = new List<Wedding>();
    }
}