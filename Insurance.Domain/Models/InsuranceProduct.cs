using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class InsuranceProduct
    {
        [Key]
        public int BenefitId { get; set; }

        [Required(ErrorMessage = "Product Id is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Benefit Name is Required")]
        [StringLength(150)]
        public string BenefitName { get; set; }

        [Required(ErrorMessage = "Base Rate is Required")]
        public decimal BaseRate { get; set; }

        [Required(ErrorMessage = "Minimum Amount is Required")]
        public decimal Minimum { get; set; }

        [Required(ErrorMessage = "Maximum Amount is Required")]
        public decimal Maximum { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct Product { get; set; }
    }
}