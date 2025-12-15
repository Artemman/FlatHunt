using System;

namespace FlatHunt.Server.Dto
{
    public class FlatDto
    {
        public int Id { get; init; }

        public string? Title { get; init; }

        public string? Address { get; init; }

        public string? City { get; init; }

        public int Rooms { get; init; }

        public decimal? Area { get; init; }

        public decimal? Price { get; init; }

        public string? Currency { get; init; }

        public bool IsActive { get; init; }

        public DateTime? CreatedAt { get; init; }

        public string? ExternalId { get; set; }
    }
}