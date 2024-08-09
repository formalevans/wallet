using System.Buffers.Text;
using System.Data;
using System.Net;
using System.Security;
using System;
using System.Text.Json.Serialization;

namespace Wallet.Models
{
public class Users
    {
        public int?  Id { get; set; }
        public string?  Username { get; set; }
        [JsonIgnore]
        public string  PasswordHash { get; set; }
        public string  Email { get; set; }
        public DateTime ?CreatedAt { get; set; }
        [JsonIgnore]
        public string?  RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime?  RefreshTokenExpiryTime { get; set; }
        [JsonIgnore]

        public ICollection<UserRole>? UserRoles { get; set; }
        [JsonIgnore]
        public ICollection<UserPermission>? UserPermissions { get; set; }
    }

}
