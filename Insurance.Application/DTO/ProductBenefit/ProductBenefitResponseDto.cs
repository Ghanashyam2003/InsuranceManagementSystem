namespace Insurance.Application.DTOs.ProductBenefit
{
    public class ProductBenefitResponseDto
    {
        public int BenefitId { get; set; }

        public int ProductId { get; set; }

        public string? BenefitName { get; set; }

        public decimal BaseRate { get; set; }

        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }
    }
}