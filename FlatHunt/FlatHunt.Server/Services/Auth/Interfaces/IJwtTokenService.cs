using FlatHunt.Server.Models;

namespace FlatHunt.Server.Services.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        Task<(string accessToken, string refreshToken)> IssueTokensAsync(User user);

        Task<(string accessToken, string refreshToken)?> RefreshAsync(string refreshToken);

        Task RevokeAsync(string refreshToken);
    }
}
