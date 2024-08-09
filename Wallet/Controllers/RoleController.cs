using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Data;
using Wallet.Models;

namespace Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly DatabaseConnection _context;

        public RoleController(DatabaseConnection context)
        {
            _context = context;
        }

        [HttpPost("create")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

   

}
