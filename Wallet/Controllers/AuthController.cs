using Microsoft.AspNetCore.Mvc;
using Wallet.Interfaces;
using Wallet.Models;
using Wallet.ViewModels;

namespace Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public AuthController(IJwtService jwtService, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // Hash the password
            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            var userLogged = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userLogged != null && BCrypt.Net.BCrypt.Verify(user.PasswordHash, userLogged.PasswordHash))
            {
                var accessToken = _jwtService.GenerateAccessToken(userLogged);
                var refreshToken = _jwtService.GenerateRefreshToken();

                userLogged.RefreshToken = refreshToken;
                userLogged.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Refresh token expiry time
                await _userRepository.SaveChangesAsync();

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var userId = principal.FindFirst("userId").Value;

            var user = await _userRepository.GetUserByIdAsync(int.Parse(userId));
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userRepository.SaveChangesAsync();

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }
    }

}
