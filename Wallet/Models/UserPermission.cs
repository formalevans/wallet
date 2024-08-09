namespace Wallet.Models
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public Users ? User { get; set; }

        public int PermissionId { get; set; }
        public Permission ? Permission { get; set; }
    }
}
