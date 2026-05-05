using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Foreverly.Models
{
    public class Wedding
    {
        public int Id { get; set; }
        public DateOnly? SuggestedDate { get; set; }
        public DateOnly? ConfirmedDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientAddress { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public int? TemplateId { get; set; }
        public Template? Template { get; set; }

        public ICollection<WeddingService> WeddingServices { get; set; } = new List<WeddingService>();
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
        public ICollection<WeddingTable> Table { get; set; } = new List<WeddingTable>();
    }
}