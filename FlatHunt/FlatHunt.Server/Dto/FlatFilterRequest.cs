namespace FlatHunt.Server.Dto
{
    public class FlatFilterRequest
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int? CityId { get; set; }

        public int? Rooms { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public decimal? MinArea { get; set; }

        public decimal? MaxArea { get; set; }

        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public string? SortDir { get; set; } = "desc";
    }
}