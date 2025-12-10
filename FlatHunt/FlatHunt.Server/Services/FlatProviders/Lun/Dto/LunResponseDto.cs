namespace FlatHunt.Server.Services.FlatProviders.Lun.Dto
{
    public class LunResponseDto
    {
        public List<LunCardDto>? Cards { get; set; }
        public int TotalRealtiesCount { get; set; }
        public int TotalGroupedCount { get; set; }
    }

    public class LunCardDto
    {
        public long Id { get; set; }

        public required string? UrlRaw { get; set; }

        public required string? Header { get; set; }

        public required string? Text { get; set; }

        public required decimal Price { get; set; }

        public required string Currency { get; set; }

        public required string Geo { get; set; }

        public DateTime InsertTime { get; set; }

        public bool IsOwner { get; set; }

        public int RoomCount { get; set; }

        public int Floor { get; set; }

        public int FloorCount { get; set; }

        public decimal? AreaTotal { get; set; }

    }
}
