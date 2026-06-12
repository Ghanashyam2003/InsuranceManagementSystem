namespace Insurance.Application.DTOs.Quote
{
    public class QuoteResponseDto
    {
        public int QuoteId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public decimal SumInsured { get; set; }

        public decimal PremiumAmount { get; set; }

        public string? RiskCategory { get; set; }

        public decimal RiskLoadingPercentage { get; set; }

        public string? Status { get; set; }
    }
}