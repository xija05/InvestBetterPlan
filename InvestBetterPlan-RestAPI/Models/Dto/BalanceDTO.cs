namespace InvestBetterPlan_RestAPI.Models.Dto
{
    public class BalanceDTO
    {
        public int UserId { get; set; }
        public int SourceCurrency { get; set; }
        public int DestinationCurrency { get; set; }
        public DateOnly LastDate { get; set; }
        public double Quotas { get; set; }
        public double Amount { get; set; }
        public double FundingShareValue { get; set; }
        public double CurrencyIndicator { get; set; }
    }
}
