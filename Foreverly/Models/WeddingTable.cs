namespace Foreverly.Models
{
    public class WeddingTable
    {
        public int Id { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; } = null!;

        public string TableName { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public string? PositionNote { get; set; }

        public ICollection<SeatingAssignment> SeatingAssignments { get; set; } = new List<SeatingAssignment>();
    }
}