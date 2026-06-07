using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.DTOs.ProductBenefit
{
    public class ProductBenefitCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string? BenefitName { get; set; }

        public decimal BaseRate { get; set; }

        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }
    }
}