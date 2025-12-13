namespace FlatHunt.Server.Dto
{
    public class CreateBrokerRequest
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? FullName { get; set; }

        public string? UserName { get; set; }

        public bool EmailConfirmed { get; set; } = true;
    }
}
