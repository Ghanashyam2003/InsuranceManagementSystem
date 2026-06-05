using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class ProductBenefit
    {
        [Key]
        public int BenefitId { get; set; }

        [Required(ErrorMessage = "Product Id is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Benefit Name is Required")]
        public string BenefitName { get; set; }

        public decimal BaseRate { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Maximum must be greater than 0")]
        public double Maximum { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum must be greater than 0")]
        public double Minimum { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct Product { get; set; }
    }
}