namespace Wallet.Models
{
    public class Budget
    {
        public int BudgetId { get; set; }
        public decimal Limit { get; set; }
        public decimal Spent { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
    }

}
