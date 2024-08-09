using System.Text.Json.Serialization;
using Wallet.Models;

namespace Wallet.ViewModels
{
    public class UserModel
    {
        public string Username { get; set; }=string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
        
        public ICollection<UserPermission>? UserPermissions { get; set; }
    }
}
