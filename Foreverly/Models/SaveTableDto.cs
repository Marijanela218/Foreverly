namespace Foreverly.Models
{
    public class SaveTableDto
    {
        public int Id { get; set; }
        public int WeddingId { get; set; }
        public string TableName { get; set; }
        public int Capacity { get; set; }
        public string? Side { get; set; }
    }
}
