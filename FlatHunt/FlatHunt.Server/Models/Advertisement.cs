namespace FlatHunt.Server.Models
{
    public class Advertisement : Entity
    {
        public FlatSourceType FlatSourceType { get; set; }

        public required string ExternalId { get; set; }

        public required string Url { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Currency { get; set; }

        public string? Address { get; set; }

        public string? RawData { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsOwner { get; set; }

        public Flat? Flat { get; set; }

        public int? FlatId { get; set; }

        //TODO add geo, contacts, meta data hus/electricity etc, commission
    }
}
