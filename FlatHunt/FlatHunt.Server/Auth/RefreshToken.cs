namespace FlatHunt.Server.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RevokedAt { get; set; }

        public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
    }
}
