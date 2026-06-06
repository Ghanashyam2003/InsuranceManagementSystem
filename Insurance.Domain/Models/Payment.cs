using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insurance.Domain.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Policy Id is Required")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Amount is Required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Mode is Required")]
        [StringLength(50)]
        public string? PaymentMode { get; set; }

        [Required(ErrorMessage = "Payment Status is Required")]
        [StringLength(30)]
        public string? PaymentStatus { get; set; } = "Pending";

        [Required(ErrorMessage = "Transaction Number is Required")]
        [StringLength(100)]
        public string? TransactionNumber { get; set; }

        [Required(ErrorMessage = "Payment Date is Required")]
        public DateTime PaymentDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("PolicyId")]
        public Policy? Policy { get; set; }

    }
}