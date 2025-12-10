namespace FlatHunt.Server.Services.Parser.Models
{
    public class FlatParseFilter
    {
        public int RoomCount { get; set; } = 1;

        public int CityId { get; set; }

        public DateTime CreatedFrom { get; set; }

        public bool WithOwner { get; set; }

        //TODO move to pagination model
        public int Page { get; set; }
    }
}
