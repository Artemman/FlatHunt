using System.ComponentModel.DataAnnotations;

namespace FlatHunt.Server.Dto
{
    public record LoginUserDto([Required] string Email, [Required] string Password);
}
