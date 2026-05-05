namespace Foreverly.Models
{
    public class PartnerCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Partner> Partners { get; set; } = new List<Partner>();
    }
}