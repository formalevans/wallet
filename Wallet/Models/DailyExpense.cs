using Wallet.Entities;
using Wallet.Enum;

namespace Wallet.Models
{
    
        public class DailyExpense : BaseEntity
        {

            public string TransactionName { get; set; } = default!;

            public string VendorName { get; set; } = default!;

            public int TransactionCost { get; set; } = default!;

            public int TransactionCostCharges { get; set; } = default!;

            public ModeOfPayments ModeOfPayment { get; set; } = default!;
            public DateTime DateOfTransaction { get; set; } = DateTime.Now;

            public Categories Category { get; set; } = default!;

            public int TotalAmount
            {
                get
                {
                    return TransactionCostCharges + TransactionCost;
                }
                set { }
            }

        }

   
}
