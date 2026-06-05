
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Quote
    {
        [Key]
        public int QuoteId { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Product Id is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Premium Amount is Required")]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = "Quote Date is Required")]
        public DateTime QuoteDate { get; set; }

        [Required(ErrorMessage = "Expiry Date is Required")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        [StringLength(30)]
        public string? Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("ProductId")]
        public InsuranceProduct? Product { get; set; }
    }
}