namespace Foreverly.Models
{
    public class TemplateItem
    {
        public int Id { get; set; }

        public int TemplateId { get; set; }
        public Template Template { get; set; } = null!;

        public string ItemName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public bool IsRequired { get; set; }
    }
}