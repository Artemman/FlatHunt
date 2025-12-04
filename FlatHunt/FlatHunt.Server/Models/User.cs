using Microsoft.AspNetCore.Identity;

namespace FlatHunt.Server.Models
{
    public class User : IdentityUser<int>
    {
        public string? FullName { get; set; }

        public bool Deleted { get; set; }
    }
}
