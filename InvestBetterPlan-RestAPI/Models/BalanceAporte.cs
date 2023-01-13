namespace InvestBetterPlan_RestAPI.Models
{
    public class BalanceAporte
    {
        public int SourceCurrencyId { get; set; }
        public int DestinationCurrencyId { get; set; }
        public DateOnly LastDate { get; set; }
        public double Quotas { get; set; }
        public double Amount { get; set; }
        public double FundingShareValue { get; set; }
        public double CurrencyIndicator { get; set; }
        public string Type { get; set; }
        public bool IsBox { get; set; } 
    }
}
