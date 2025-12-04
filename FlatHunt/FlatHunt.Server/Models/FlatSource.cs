namespace FlatHunt.Server.Models
{
    public enum FlatSourceType
    {
        None, 
        Lun,
        FlatFy
    }

    public class FlatSource
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public FlatSourceType Type { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
