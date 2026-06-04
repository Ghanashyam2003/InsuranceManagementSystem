using InsuranceProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Entities
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Policy Number is Required")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Product Id is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Sum Insured is Required")]
        public decimal SumInsured { get; set; }

        [Required(ErrorMessage = "Policy Start Date is Required")]
        public DateTime PolicyStartDate { get; set; }

        [Required(ErrorMessage = "Policy End Date is Required")]
        public DateTime PolicyEndDate { get; set; }

        [Required(ErrorMessage = "Premium Amount is Required")]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerMaster Customer { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct Product { get; set; }
    }
}