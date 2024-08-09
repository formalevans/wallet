namespace Wallet.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
    }

}
