namespace Wallet.Enum
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer
    }

    public enum Categories
    {
        None = 0,
        Transport = 1,
        Food,
        Other
    }
    public enum ModeOfPayments
    {
        None = 0,
        Mpesa = 1,
        Bank,
        Cash
    }
}
