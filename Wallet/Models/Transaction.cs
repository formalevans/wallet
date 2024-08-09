using Wallet.Enum;

namespace Wallet.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public TransactionType Type { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
    }

}
