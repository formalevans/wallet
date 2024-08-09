using System.Security.Claims;
using Wallet.Models;

namespace Wallet.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(Users user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
