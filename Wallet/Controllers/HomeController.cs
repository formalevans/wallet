using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Wallet.ViewModels;
using Wallet.Interfaces;

namespace Wallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DatabaseConnection _dbContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public HomeController(DatabaseConnection dbContext, ILogger<HomeController> logger, IConfiguration configuration,IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _logger = logger;
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<Users>>> GetAllUsers()
        {
            if (_dbContext.Users == null)
            {
                return NotFound("Database context is null.");
            }

         
            var users = await _dbContext.Users.ToListAsync();
            return users.Count == 0 ? NotFound("No users found.") : Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> CreateUser([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("User cannot be null.");
            }
            Users user = new Users();
            user.Username = userModel.Username;
            user.PasswordHash = userModel.PasswordHash;
            user.Email = userModel.Email;
            user.CreatedAt = DateTime.UtcNow;


            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            if (_dbContext.Users == null)
            {
                return NotFound("Database context is null.");
            }

            var user = await userRepository.GetUserByIdAsync(id);
            return user == null ? NotFound("User not found.") : Ok(user);
            }

            [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Users user)
        {
            if (user == null || id != user.Id)
            {
                return BadRequest("User data is invalid.");
            }

            _dbContext.Entry(user).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("User not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Users user)
        {
           var userLogged= await _dbContext.Users.FirstOrDefaultAsync(u => u.Email==user.Email && u.PasswordHash==user.PasswordHash);
            if (userLogged != null)
            {
                var claims = new[]
                {
                     new Claim(JwtRegisteredClaimNames.Sub,configuration["JWT:Subject"]),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                     new Claim("userId",user.Id.ToString()),
                     new Claim("Email",user.Email,ToString())

                 };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["JWT:Issuer"],
                    configuration["JWT:Audience"],
                    claims,
                    signingCredentials:signIn,
                    expires:DateTime.UtcNow.AddSeconds(60)
                    );
                var tokenValue=new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new {Token=tokenValue,User=userLogged});

            }
            return NoContent();
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> PatchUser(int id, [FromBody] Users user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest("User cannot be null.");
        //    }

        //    var existingUser = await _dbContext.Users.FindAsync(id);
        //    if (existingUser == null)
        //    {
        //        return NotFound("User not found.");
        //    }
        //    if (!string.IsNullOrEmpty(user.Name)) existingUser.Name = user.Name;
        //    if (!string.IsNullOrEmpty(user.Email)) existingUser.Email = user.Email;
        //    if (!string.IsNullOrEmpty(user.Password)) existingUser.Password = user.Password;
        //    if (!string.IsNullOrEmpty(user.Username)) existingUser.Username = user.Username;
        //    if (!string.IsNullOrEmpty(user.PhoneNumber)) existingUser.PhoneNumber = user.PhoneNumber;

        //    _dbContext.Entry(existingUser).State = EntityState.Modified;
        //    try
        //    {
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound("User not found.");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
    }
}
