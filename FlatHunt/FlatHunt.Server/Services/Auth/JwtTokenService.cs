using FlatHunt.Server.Auth;
using FlatHunt.Server.Data;
using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Auth.Interfaces;
using FlatHunt.Server.Services.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlatHunt.Server.Services.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;

        public JwtTokenService(IOptionsSnapshot<JwtOptions> jwtOptions,
            UserManager<User> userManager,
            AppDbContext dbContext)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<(string accessToken, string refreshToken)> IssueTokensAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = GetUserClaims(user, roles);

            var signingCredentials = GetSigningCredentials();

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                            issuer: _jwtOptions.Issuer,
                            audience: _jwtOptions.Audience,
                            claims: claims,
                            notBefore: now,
                            expires: now.AddMinutes(_jwtOptions.AccessTokenMinutes),
                            signingCredentials: signingCredentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            _dbContext.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = now.AddDays(_jwtOptions.RefreshTokenDays),
            });

            await _dbContext.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, string refreshToken)?> RefreshAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (token is null
                || !token.IsActive)
            {
                return null;
            }

            var user = await _dbContext.Users.FindAsync(token.UserId);
            if (user is null
                || user.Deleted)
            {
                return null;
            }

            token.RevokedAt = DateTime.UtcNow;
            var (newAccessToken, newRefreshToken) = await IssueTokensAsync(user);
            await _dbContext.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }

        public async Task RevokeAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (token is null
                || !token.IsActive)
            {
                return;
            }

            token.RevokedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        private static List<Claim> GetUserClaims(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Name, user.UserName ?? string.Empty)
            }
            ;
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
