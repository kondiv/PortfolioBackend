namespace Domain.Entities
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public DateTime? Revoked { get; set; }
        public string? ReplacedByToken { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
