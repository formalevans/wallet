using Wallet.Interfaces;
using Wallet.Models;
using Wallet.Data;
using Microsoft.EntityFrameworkCore;
namespace Wallet.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection _context;

        public UserRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
