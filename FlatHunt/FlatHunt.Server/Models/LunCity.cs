namespace FlatHunt.Server.Models
{
    public class LunCity : Entity
    {
        public required string Name { get; set; }

        public required string GeoId { get; set; }

        public required string GeoType { get; set; }

        public required string Url { get; set; }

    }
}
