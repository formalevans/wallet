using Wallet.Models;

namespace Wallet.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByIdAsync(int id);
        Task AddUserAsync(Users user);
        Task SaveChangesAsync();
    }
}
