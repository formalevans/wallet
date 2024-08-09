namespace Wallet.Models
{
    public class TokenApiModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AccessTokenExpiryTime { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
