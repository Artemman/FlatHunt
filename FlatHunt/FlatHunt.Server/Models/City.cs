namespace FlatHunt.Server.Models
{
    public class City
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int? LunCityId { get; set; }

        public int? FlatFyCityId { get; set; }

        public LunCity? LunCity { get; set; }
    }
}
