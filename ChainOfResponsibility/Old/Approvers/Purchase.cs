
namespace DesignPatterns.ChainOfResponsibility.Approvers
{
    // Simple class holding request details

    class Purchase
    {
        public Purchase(int number, double amount, string purpose)
        {
            Number = number;
            Amount = amount;
            Purpose = purpose;
        }

        public int Number { get; }
        public double Amount { get; }
        public string Purpose { get; }
    }
}
