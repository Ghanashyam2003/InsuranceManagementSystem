using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        public int? QuoteId { get; set; }

        [Required(ErrorMessage = "Policy Number is Required")]
        [StringLength(50)]
        public string? PolicyNumber { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Product Id is Required")]
        public int ProductId { get; set; }

        public int? AgentId { get; set; }

        [Required(ErrorMessage = "Sum Insured is Required")]
        public decimal SumInsured { get; set; }

        [Required(ErrorMessage = "Policy Start Date is Required")]
        public DateTime PolicyStartDate { get; set; }

        [Required(ErrorMessage = "Policy End Date is Required")]
        public DateTime PolicyEndDate { get; set; }

        [Required(ErrorMessage = "Premium Amount is Required")]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        [StringLength(30)]
        public string? Status { get; set; } = "Pending";

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("QuoteId")]
        public Quote? Quote { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct? Product { get; set; }

        [ForeignKey("AgentId")]
        public Agents? Agent { get; set; }
    }
}